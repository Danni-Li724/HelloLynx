using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CollisionManager : MonoBehaviour
{
   public static CollisionManager Instance;

    [Header("Scoring/UI")]
    public int collisionCount = 0;
    public int successfulReactions = 0;
    public Text collisionText;
    public Text scoreText;

    [Header("FX & Timing")]
    public ParticleSystem reactionEffectPrefab;
    [Tooltip("Time (seconds) after a collision during which input counts")]
    public float inputWindow = 1f;

    private List<CollisionEvent> collisionEvents = new List<CollisionEvent>();
    private bool gameActive = false;

    [System.Serializable]
    private class CollisionEvent
    {
        public SnowMovement snow;   // the specific sprite that collided
        public Vector3 position;
        public float time;
        public bool reacted;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        enabled = false; // only tick during the minigame
    }

    public void BeginMinigame()
    {
        collisionCount = 0;
        successfulReactions = 0;
        if (collisionText) collisionText.text = $"Collisions: {collisionCount}";
        if (scoreText) scoreText.text = $"Score: {successfulReactions}";

        collisionEvents.Clear();
        gameActive = true;
        enabled = true;
    }

    public void ResetMinigame() => BeginMinigame();

    private void Update()
    {
        if (!gameActive) return;

        // Walk backwards so we can remove safely.
        for (int i = collisionEvents.Count - 1; i >= 0; i--)
        {
            var evt = collisionEvents[i];
            if (!evt.reacted && Time.time - evt.time > inputWindow)
            {
                // MISS: flash red then destroy this specific sprite
                if (evt.snow != null)
                    evt.snow.OnReactMissed(0.5f);

                collisionEvents.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Register a new collision for a specific SnowMovement instance.
    /// </summary>
    public void RegisterCollision(SnowMovement snow, Vector3 position)
    {
        collisionCount++;
        if (collisionText) collisionText.text = $"Collisions: {collisionCount}";

        collisionEvents.Add(new CollisionEvent
        {
            snow = snow,
            position = position,
            time = Time.time,
            reacted = false
        });
    }

    /// <summary>
    /// Called by your input system. Consumes the oldest valid event in window.
    /// </summary>
    public void TryReact()
    {
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            var evt = collisionEvents[i];
            if (!evt.reacted && Time.time - evt.time <= inputWindow)
            {
                evt.reacted = true;

                successfulReactions++;
                if (scoreText) scoreText.text = $"Score: {successfulReactions}";

                // Success FX
                if (reactionEffectPrefab != null)
                    Instantiate(reactionEffectPrefab, evt.position, Quaternion.identity);

                // Remove the sprite immediately on success
                if (evt.snow != null)
                    evt.snow.OnReactSuccess();

                // Remove the event
                collisionEvents.RemoveAt(i);
                return;
            }
        }
    }

    public bool PlayerWon()
    {
        return collisionCount > 0 && ((float)successfulReactions / collisionCount >= 0.6f);
    }

    public void FinishMinigame()
    {
        gameActive = false;
        enabled = false;

        bool won = PlayerWon();
        // Notify controller (kept as-is)
        FindObjectOfType<MinigameControllerBase>()?.OnMinigameComplete(won);
    }
}