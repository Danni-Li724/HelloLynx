using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/BadgeSpriteLibrary")]
public class BadgeSpriteLibrary : ScriptableObject
{
    [System.Serializable]
    public struct BadgeSpriteEntry
    {
        public BadgeType badgeType;
        public Sprite sprite;
    }
    public BadgeSpriteEntry[] badgeSprites;

    public Sprite GetSprite(BadgeType type)
    {
        foreach (var entry in badgeSprites)
            if (entry.badgeType == type) return entry.sprite;
        return null;
    }
}