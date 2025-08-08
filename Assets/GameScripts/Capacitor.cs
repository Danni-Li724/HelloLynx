using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

public class Capacitor : MonoBehaviour
{
    [Header("Spawn Setup")]
    public HoppingElectricity electricityPrefab; 
    public Transform[] waypoints;

    [Header("Spawn Settings")]
    public bool autoStart = false;
    public float spawnInterval = 2f;
    public int maxToSpawn = 10;

    private int spawnedCount = 0;
    private Coroutine spawnRoutine;
    public bool IsSpawning => spawnRoutine != null;

    void Start()
    {
        if (autoStart) StartSpawning();
    }

    public void StartSpawning()
    {
        if (IsSpawning) return;
        if (electricityPrefab == null || waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning($"[{name}] Capacitor missing prefab or waypoints.");
            return;
        }
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (!IsSpawning) return;
        StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    public void ResetAndStart()
    {
        StopSpawning();
        spawnedCount = 0;
        StartSpawning();
    }

    private IEnumerator SpawnRoutine()
    {
        while (spawnedCount < maxToSpawn)
        {
            SpawnHopper();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
        spawnRoutine = null; 
    }

    private void SpawnHopper()
    {
        var hopper = Instantiate(electricityPrefab, waypoints[0].position, Quaternion.identity);
        hopper.waypoints = waypoints;
        hopper.loop = false;
        hopper.faceDirection = true;
    }
}
