using UnityEngine;

public class RAMSlot : MonoBehaviour
{
    [System.NonSerialized]
    public string slotAddress; 
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(string address)
    {
        slotAddress = address.ToLower();
    }

    public void HighlightWhenMatched()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
        }
    }
    
    public void HighlightWhenIncorrect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
    }
    
    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        
            // Draw the slot address as text in Scene view
            UnityEditor.Handles.Label(transform.position, slotAddress);
        }
    }
    
}
