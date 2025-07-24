using UnityEngine;

public class YSortHelper : MonoBehaviour
{
    public static void ApplyYSort(SpriteRenderer spriteRenderer, Transform transform, int sortingOffset = 0)
    {
        if (spriteRenderer == null) return;

        // x -100 to flip the Y axis for correct layering
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + sortingOffset;
    }
}
