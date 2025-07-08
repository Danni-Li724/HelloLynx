using UnityEngine;

public class SnowSpawner : MonoBehaviour
{
    public GameObject snowPrefab;
    public Transform[] spawnPoints;
    public float spawnRate = 1f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / spawnRate)
        {
            int index = Random.Range(0, spawnPoints.Length);
            Instantiate(snowPrefab, spawnPoints[index].position, Quaternion.identity);
            timer = 0f;
        }
    }
}
