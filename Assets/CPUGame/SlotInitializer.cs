using UnityEngine;

public class SlotInitializer : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject slotPrefab;
    public Transform slotParent;
    public int numberOfColumns = 16;
    public int numberOfRows = 4;
    public float spacingX = 1.2f;
    public float spacingY = 1.2f;
    public Vector2 gridCenterPosition = Vector2.zero;

    [Header("Row Labels (Match Row Count)")]
    public string[] rowLabels = new string[] { "a", "b", "c", "d" };

    private void Start()
    {
        if (rowLabels.Length != numberOfRows)
        {
            Debug.LogWarning("Row label count doesn't match row count. Adjusting automatically.");
            rowLabels = new string[numberOfRows];
            for (int i = 0; i < numberOfRows; i++)
                rowLabels[i] = ((char)('a' + i)).ToString();
        }

        // calculate centering offset
        float offsetX = (numberOfColumns - 1) * spacingX * 0.5f;
        float offsetY = (numberOfRows - 1) * spacingY * 0.5f;

        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 1; col <= numberOfColumns; col++)
            {
                float x = (col - 1) * spacingX - offsetX + gridCenterPosition.x;
                float y = -row * spacingY + offsetY + gridCenterPosition.y;
                Vector3 pos = new Vector3(x, y, 0);

                GameObject slot = Instantiate(slotPrefab, pos, Quaternion.identity, slotParent);
                slot.GetComponent<RAMSlot>().Initialize(rowLabels[row] + col.ToString());
            }
        }
    }
}
