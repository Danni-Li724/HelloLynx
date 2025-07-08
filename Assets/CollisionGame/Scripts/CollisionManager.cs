using UnityEngine;
using UnityEngine.UI;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    public int collisionCount = 0;
    public Text collisionText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterCollision()
    {
        collisionCount++;
        if (collisionText != null)
            collisionText.text = "Collisions: " + collisionCount;
    }
}
