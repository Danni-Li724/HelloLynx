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

    private float spawnTimer = 0f;  // Internal timer to keep track of spawn timing
    private Queue<GameObject> trackQueue = new(); // Queue to keep track of all active bytes
    public Transform redLineTrigger;  
    private bool hogged = false;  // if something is sitting over the red line
    private float hogTimer = 0f;  // Countdown before punishing the player
    private const float HOG_DURATION = 5f; // How long a byte can hog the line before it's a problem

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
    }

    private void Start()
    {
        timeLeft = totalGameTime;
        UpdateTimerUI();
    }
    private void Update()
    {
        if (gameEnded) return;
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0) timeLeft = 0f;
        UpdateTimerUI();
        
        float t = 1 - Mathf.Clamp01(timeLeft / frequencyRampTime);
        float currentSpawnInterval = Mathf.Lerp(minSpawnInterval, t, frequencyRampTime * t);
        
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnByte();
            spawnTimer = 0;
        }
        // Check if a byte is stuck past the red line
        CheckTrackHog();

        if (timeLeft <= 0)
        {
            EndGame(true);
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + timeLeft.ToString("0.0");
        }
    }

    private void EndGame(bool timerExpired)
    {
        gameEnded = true;
        if (timerExpired)
        {
            if (mistakes < maxMistakes)
            {
                warningText.text = "You Win!";
            }
            else
            {
                warningText.text = "GameOver!";
            }
        }
        else
        {
            warningText.text = "GameOver";
        }

        Time.timeScale = 0;
    }

    void SpawnByte()
    {
        // Make a new random address for the byte
        string address = GenerateRandomAddress();
        Debug.Log($"[SpawnByte] Generated address: {address}");

        // Create a new byte at the spawn point
        GameObject byteObj = Instantiate(bytePrefab, spawnPoint.position, Quaternion.identity);
        // Give it its target address and tracking target
        BytePacket packet = byteObj.GetComponent<BytePacket>();
        packet.targetAddress = address;
        //packet.trackTarget = trackTargets[Random.Range(0, trackTargets.Length)];
        packet.waypoints = new Transform[] { trackTargets[0], trackTargets[1] };
        packet.currentWaypointIndex = 0;
        packet.movingForward = true;
        Debug.Log($"[SpawnByte] BytePacket targetAddress set to: {packet.targetAddress}");

        // Keep track of the byte so we can monitor it
        trackQueue.Enqueue(byteObj);
    }

    string GenerateRandomAddress()
    {
        // Pick a random row and column, return it as a string like "b12"
        string[] rowLabels = { "a", "b", "c", "d" };
        string rowLabel = rowLabels[Random.Range(0, 4)];
        int columnNumber = Random.Range(1, 17);
        string address = rowLabel + columnNumber.ToString();
        Debug.Log($"[GenerateRandomAddress] Row: {rowLabel}, Column: {columnNumber}, Final: {address}");
        return address;
    }

    void CheckTrackHog()
    {
        int overLine = 0;

        // See how many bytes are past the red line
        foreach (GameObject obj in trackQueue)
        {
            if (obj == null) continue;

            if (obj.transform.position.y <= redLineTrigger.position.y)
                overLine++;
        }

        if (overLine > 0)
        {
            // Start hog timer if one or more bytes are hogging the line
            if (!hogged)
            {
                hogged = true;
                hogTimer = HOG_DURATION;
            }
            else
            {
                hogTimer -= Time.deltaTime;
                if (hogTimer <= 0)
                {
                    // Time's up — punish the player
                    RegisterIncorrectAllocation();
                    hogged = false;
                }
            }
        }
        else
        {
            // Reset if no bytes are over the line
            hogged = false;
        }
    }

    public void RegisterCorrectAllocation(BytePacket packet)
    {
        Debug.Log($"[RegisterCorrectAllocation] Correct allocation for address: {packet.targetAddress}");
        // +1 to score and update UI
        score++;
        scoreText.text = "Score: " + score;
        // Remove the byte from the queue
        trackQueue = new Queue<GameObject>(trackQueue.Where(x => x != packet.gameObject));
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
