using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CPUGameManager : MonoBehaviour
{
    public static CPUGameManager Instance;

    public int score = 0;
    public int mistakes = 0;
    public int maxMistakes = 5;
    public Text scoreText;
    public Text warningText;

    public GameObject bytePrefab;
    public GameObject addressPrefab;
    public Transform spawnPoint;
    public Transform[] trackTargets;
    public float spawnInterval = 3f;

    private float spawnTimer = 0f;
    private Queue<GameObject> trackQueue = new();
    public Transform redLineTrigger;
    private bool hogged = false;
    private float hogTimer = 0f;
    private const float HOG_DURATION = 5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnByte();
            spawnTimer = 0;
        }

        CheckTrackHog();
    }

    void SpawnByte()
    {
        string address = GenerateRandomAddress();
        Debug.Log($"[SpawnByte] Generated address: {address}");
        
        GameObject byteObj = Instantiate(bytePrefab, spawnPoint.position, Quaternion.identity);
        BytePacket packet = byteObj.GetComponent<BytePacket>();
        packet.targetAddress = address;
        packet.trackTarget = trackTargets[Random.Range(0, trackTargets.Length)];

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
        int overLine = 0;
        foreach (GameObject obj in trackQueue)
        {
            if (obj == null) continue;
            if (obj.transform.position.y <= redLineTrigger.position.y)
                overLine++;
        }

        if (overLine > 0)
        {
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
                    RegisterIncorrectAllocation();
                    hogged = false;
                }
            }
        }
        else
        {
            hogged = false;
        }
    }

    public void RegisterCorrectAllocation(BytePacket packet)
    {
        Debug.Log($"[RegisterCorrectAllocation] Correct allocation for address: {packet.targetAddress}");
        score++;
        scoreText.text = "Score: " + score;
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
            Time.timeScale = 0;
        }
    }
}
