using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Analytics;
using Random = UnityEngine.Random;

public class CPUGameManager : MonoBehaviour
{
    public static CPUGameManager Instance; 
    public int score = 0;                
    public int mistakes = 0;              
    public int maxMistakes = 5;           
    public Text scoreText;                 
    public Text warningText;               

    public GameObject bytePrefab;  
    public GameObject addressPrefab; // Unused yet
    public Transform spawnPoint; // Where bytes spawn
    public Transform[] trackTargets;  // Random destinations for spawned bytes
    public float spawnInterval = 3f; 
    public GameObject stallText;

    private float spawnTimer = 0f;  // Internal timer to keep track of spawn timing
    private Queue<GameObject> trackQueue = new(); // Queue to keep track of all active bytes
    public Transform redLineTrigger;  
    private bool hogged = false;  // if something is sitting over the red line
    private float hogTimer = 0f;  // Countdown before punishing the player
    [SerializeField] private const float HOG_DURATION = 3f; 
    private readonly Dictionary<GameObject, float> hogCountdowns = new();
    private readonly Dictionary<GameObject, Vector3> lastPositions = new(); // NEW: track motion per byte
    private const float STILLNESS_EPSILON_SQR = 0.0004f;
    
    private Action<bool> onGameOverCallback;

    [Header("Timer Settings")] 
    public float totalGameTime = 60f;
    public Text timerText;
    public float minSpawnInterval = 0.6f;
    public float frequencyRampTime = 40f;
    private float timeLeft;
    private bool gameEnded = false;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        enabled = false;
    }

    private void Start()
    {
        //timeLeft = totalGameTime;
        UpdateTimerUI();
        SpawnByte();
    }
    private void Update()
    {
        if (gameEnded) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) timeLeft = 0f;
        UpdateTimerUI();

        // FIX: ramp from initial spawnInterval down to minSpawnInterval as the timer runs out
        float t = 1f - Mathf.Clamp01(timeLeft / frequencyRampTime);
        float currentSpawnInterval = Mathf.Lerp(spawnInterval, minSpawnInterval, t);

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnInterval)   // <-- use the ramped value
        {
            SpawnByte();
            spawnTimer = 0f;
        }

        CheckTrackHog();

        if (mistakes >= maxMistakes && !gameEnded)
        {
            EndGame(timerExpired: false);
            return;
        }

        if (timeLeft <= 0 && !gameEnded)
        {
            EndGame(timerExpired: true);
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + timeLeft.ToString("0.0");
        }
    }
    
    public void BeginMinigame(Action<bool> onGameOver)
    {
        // Reset game state and assign callback
        onGameOverCallback = onGameOver;
        score = 0;
        mistakes = 0;
        timeLeft = totalGameTime;
        gameEnded = false;
        spawnTimer = 0f;
        hogged = false;
        hogTimer = 0f;
        // Destroy all bytes from last round
        foreach (var obj in trackQueue) { if (obj) Destroy(obj); }
        trackQueue.Clear();
        Time.timeScale = 1f; // Unpause game

        // UI
        if (scoreText) scoreText.text = "Score: 0";
        if (warningText) warningText.text = "";
        if (timerText) timerText.text = $"Time: {timeLeft:0.0}";
        if (stallText) stallText.SetActive(false); 
        
        ResetAllRamSlots();

        enabled = true; // Enable Update loop for timer
    }
    
    private void ResetAllRamSlots()
    {
        var slots = FindObjectsOfType<RAMSlot>(true); // include inactive
        for (int i = 0; i < slots.Length; i++)
            slots[i].ClearHighlight();
    }

    // --- Call this to retry the minigame ---
    public void ResetMinigame()
    {
        BeginMinigame(onGameOverCallback);
    }

    private void EndGame(bool timerExpired)
    {
        gameEnded = true;
        enabled = false;

        bool playerWon = false;
        if (timerExpired)
        {
            playerWon = mistakes < maxMistakes;
            if (warningText) warningText.text = playerWon ? "You Win!" : "Game Over!";
        }
        else
        {
            playerWon = false;
            if (warningText) warningText.text = "Game Over!";
        }

        if (timerText) timerText.text = "Time: 0.0";

        // Clean up all leftover bytes on screen
        DestroyAllBytes();

        Time.timeScale = 1f; // ensure we're unpaused for any UI flow
        onGameOverCallback?.Invoke(playerWon);
    }

    void SpawnByte()
    {
        string address = GenerateRandomAddress();
        Debug.Log($"[SpawnByte] Generated address: {address}");
        GameObject byteObj = Instantiate(bytePrefab, spawnPoint.position, Quaternion.identity);
        BytePacket packet = byteObj.GetComponent<BytePacket>();
        packet.targetAddress = address;
        //packet.trackTarget = trackTargets[Random.Range(0, trackTargets.Length)];
        packet.waypoints = new Transform[] { trackTargets[0], trackTargets[1] };
        packet.currentWaypointIndex = 0;
        packet.movingForward = true;
        Debug.Log($"[SpawnByte] BytePacket targetAddress set to: {packet.targetAddress}");
        trackQueue.Enqueue(byteObj);
    }

    string GenerateRandomAddress()
    {
        string[] rowLabels = { "a", "b", "c", "d" };
        string rowLabel = rowLabels[Random.Range(0, 4)];
        int columnNumber = Random.Range(1, 17);
        string address = rowLabel + columnNumber.ToString();
        Debug.Log($"[GenerateRandomAddress] Row: {rowLabel}, Column: {columnNumber}, Final: {address}");
        return address;
    }

    void CheckTrackHog()
    {
         bool anyCounting = false;

    foreach (GameObject obj in trackQueue.ToList())
    {
        if (obj == null) continue;

        var pkt = obj.GetComponent<BytePacket>();
        if (pkt == null) continue;

        // 1) Never punish while the player is dragging this byte
        if (pkt.IsDragging)
        {
            hogCountdowns.Remove(obj);
            lastPositions[obj] = obj.transform.position;
            continue;
        }

        // 2) Must be at/under the red line
        if (obj.transform.position.y > redLineTrigger.position.y)
        {
            hogCountdowns.Remove(obj);
            lastPositions[obj] = obj.transform.position;
            continue;
        }

        // 3) Only count down if the byte is effectively *still*
        Vector3 pos = obj.transform.position;
        if (!lastPositions.TryGetValue(obj, out var prev)) prev = pos;

        float movedSqr = (pos - prev).sqrMagnitude;
        lastPositions[obj] = pos;

        if (!hogCountdowns.ContainsKey(obj))
            hogCountdowns[obj] = HOG_DURATION;

        if (movedSqr <= STILLNESS_EPSILON_SQR)
        {
            hogCountdowns[obj] -= Time.deltaTime;
            anyCounting = true;

            if (hogCountdowns[obj] <= 0f)
            {
                RegisterIncorrectAllocation();

                stallText?.SetActive(true); // optional flair at the moment of punish

                var bytePkt = obj.GetComponent<BytePacket>();
                if (bytePkt != null) bytePkt.FlashRedAndDestroy(1f); // your existing behavior

                RemoveFromTrackQueue(obj);
                hogCountdowns.Remove(obj);
                lastPositions.Remove(obj);
            }
        }
        else
        {
            // Moving: reset the per-byte hog timer so it only fires after being *stationary* long enough
            hogCountdowns[obj] = HOG_DURATION;
        }
    }

    // Show/hide global stall label only if at least one byte is currently being timed while still
    if (!anyCounting) stallText?.SetActive(false);
    }
    
    private void RemoveFromTrackQueue(GameObject obj)
    {
        trackQueue = new Queue<GameObject>(trackQueue.Where(x => x != null && x != obj));
    }

    private void DestroyAllBytes()
    {
        // Destroy those we're tracking
        foreach (var obj in trackQueue)
            if (obj != null) Destroy(obj);

        trackQueue.Clear();
        hogCountdowns.Clear();
        lastPositions.Clear();

        // Safety: nuke any stray BytePackets not in the queue (e.g., if something spawned outside of normal flow)
        foreach (var pkt in FindObjectsOfType<BytePacket>())
            if (pkt != null) Destroy(pkt.gameObject);
    }

    public void RegisterCorrectAllocation(BytePacket packet)
    {
        Debug.Log($"[RegisterCorrectAllocation] Correct allocation for address: {packet.targetAddress}");
        score++;
        if (scoreText) scoreText.text = "Score: " + score;

        // Remove from queue + hog tracking
        if (packet && packet.gameObject)
        {
            RemoveFromTrackQueue(packet.gameObject);
            hogCountdowns.Remove(packet.gameObject);
        }
    }

    public void RegisterIncorrectAllocation()
    {
        Debug.Log($"[RegisterIncorrectAllocation] Incorrect allocation registered");
        mistakes++;
        warningText.text = "Mistakes: " + mistakes;
        if (mistakes >= maxMistakes)
        {
            warningText.text = "Game Over";
            Time.timeScale = 0; // pause game
        }
    }
}
