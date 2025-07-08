using UnityEngine;

public class SnowMovement : MonoBehaviour
{
    public float fallSpeed = 1f;

    private float zigzagAmplitude;
    private float zigzagFrequency;
    private float spawnTime;

    private bool hasCollided = false;

    private void Start()
    {
        zigzagAmplitude = Random.Range(0.2f, 0.5f);
        zigzagFrequency = Random.Range(1f, 3f);
        spawnTime = Time.time;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        if (hasCollided) return;

        float zigzag = Mathf.Sin((Time.time - spawnTime) * zigzagFrequency) * zigzagAmplitude;
        transform.position += new Vector3(zigzag * Time.deltaTime, -fallSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCollided) return;

        if (other.CompareTag("Ground") || other.CompareTag("Girl"))
        {
            hasCollided = true;
            Debug.Log($"Snow collided with: {other.name} [{tag}]");
            CollisionManager.Instance.RegisterCollision();
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
