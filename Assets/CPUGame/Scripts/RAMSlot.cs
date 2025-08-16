using UnityEngine;

public class RAMSlot : MonoBehaviour
{
    [System.NonSerialized]
    public string slotAddress; 
    private SpriteRenderer spriteRenderer;
    public Color baseColor;

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
    
    public void ClearHighlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = baseColor;
        }
    }
    
}
