using UnityEngine;

public class SnowMovement : MonoBehaviour
{
    public float fallSpeed = 1f;

    private float zigzagAmplitude;
    private float zigzagFrequency;
    private float spawnTime;
    private bool hasCollided = false;

    private Collider2D col;
    private SpriteRenderer sr;
    private Color originalColor;

    private void Start()
    {
        zigzagAmplitude = Random.Range(0.2f, 0.5f);
        zigzagFrequency = Random.Range(1f, 3f);
        spawnTime = Time.time;

        col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;

        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
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
        if (!other.CompareTag("Ground") && !other.CompareTag("Girl")) return;

        hasCollided = true;
        if (col) col.enabled = false; // prevent duplicate triggers

        // Register THIS sprite with the manager; manager decides success/miss & destruction
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.RegisterCollision(this, transform.position);
        }
        else
        {
            // Fallback: if no manager, just destroy after a moment
            Destroy(gameObject, 0.1f);
        }
    }

    /// <summary>
    /// Called by CollisionManager when player reacted in time.
    /// </summary>
    public void OnReactSuccess()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Called by CollisionManager when input window expired with no reaction.
    /// Flashes the sprite red for 'flashDuration' seconds, then destroys.
    /// </summary>
    public void OnReactMissed(float flashDuration = 0.5f)
    {
        // Only run once even if called redundantly
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(FlashAndDestroy(flashDuration));
    }

    private System.Collections.IEnumerator FlashAndDestroy(float duration)
    {
        if (sr != null)
        {
            sr.color = Color.red;
        }
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
