using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    public int collisionCount = 0;
    public int successfulReactions = 0;

    public Text collisionText;
    public Text scoreText;
    public ParticleSystem reactionEffectPrefab;
    public float inputWindow = 1f;

    private List<CollisionEvent> collisionEvents = new List<CollisionEvent>();

    [System.Serializable]
    private class CollisionEvent
    {
        public Vector3 position;
        public float time;
        public bool reacted;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        // Remove expired collision events
        for (int i = collisionEvents.Count - 1; i >= 0; i--)
        {
            if (!collisionEvents[i].reacted && Time.time - collisionEvents[i].time > inputWindow)
            {
                collisionEvents.RemoveAt(i);
            }
        }
    }

    public void RegisterCollision(Vector3 position)
    {
        collisionCount++;
        collisionText.text = $"Collisions: {collisionCount}";
        collisionEvents.Add(new CollisionEvent
        {
            position = position,
            time = Time.time,
            reacted = false
        });
    }

    public void TryReact()
    {
        foreach (var evt in collisionEvents)
        {
            if (!evt.reacted && Time.time - evt.time <= inputWindow)
            {
                evt.reacted = true;
                successfulReactions++;
                scoreText.text = $"Score: {successfulReactions}";

                if (reactionEffectPrefab != null)
                {
                    Instantiate(reactionEffectPrefab, evt.position, Quaternion.identity);
                }

                return;
            }
        }
    }

    public bool PlayerWon()
    {
        return collisionCount > 0 && ((float)successfulReactions / collisionCount >= 0.6f);
    }
}