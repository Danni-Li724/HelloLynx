using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

public class Capacitor : MonoBehaviour
{
    public HoppingElectricity electricityPrefab; 
    public Transform[] waypoints;           
    public float spawnInterval = 2f;
    public int maxToSpawn = 10;

    private int spawnedCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (spawnedCount < maxToSpawn)
        {
            SpawnHopper();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnHopper()
    {
        if (electricityPrefab == null || waypoints == null || waypoints.Length == 0) return;

        var hopper = Instantiate(electricityPrefab, waypoints[0].position, Quaternion.identity);
        hopper.waypoints = waypoints;
        hopper.loop = false;
        hopper.faceDirection = true;
    }
}
