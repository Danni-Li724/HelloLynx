using UnityEngine;

public class SlotInitializer : MonoBehaviour
{
   [Header("Grid Settings")]
    public GameObject slotPrefab; // The prefab that will be used for each RAM slot
    public Transform slotParent; // Slot container
    public int numberOfColumns = 16; // Number of columns in the slot grid
    public int numberOfRows = 4;  // Number of rows in the slot grid
    public float spacingX = 1.2f; // Horizontal space between slots
    public float spacingY = 1.2f;  // Vertical space between slots
    public Vector2 gridCenterPosition = Vector2.zero; // Where to center the whole grid

    [Header("Row Labels (Match Row Count)")]
    public string[] rowLabels = new string[] { "a", "b", "c", "d" }; // Labels for each row

    private void Start()
    {
        // If the row label count doesn't match the number of rows, auto-generate them
        if (rowLabels.Length != numberOfRows)
        {
            Debug.LogWarning("Row label count doesn't match row count. Adjusting automatically.");
            rowLabels = new string[numberOfRows];
            for (int i = 0; i < numberOfRows; i++)
                rowLabels[i] = ((char)('a' + i)).ToString(); // generates "a", "b", etc.
        }

        // Figure out how much to shift the grid so it's centered
        float offsetX = (numberOfColumns - 1) * spacingX * 0.5f;
        float offsetY = (numberOfRows - 1) * spacingY * 0.5f;

        // Loop through each row and column to place all the slots
        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 1; col <= numberOfColumns; col++)
            {
                // Calculate where this slot should go in world space
                float x = (col - 1) * spacingX - offsetX + gridCenterPosition.x;
                float y = -row * spacingY + offsetY + gridCenterPosition.y;
                Vector3 pos = new Vector3(x, y, 0);

                // Actually make the slot and parent it
                GameObject slot = Instantiate(slotPrefab, pos, Quaternion.identity, slotParent);

                // Set up the slot’s RAM address
                RAMSlot ramSlot = slot.GetComponent<RAMSlot>();
                string fullAddress = rowLabels[row] + col.ToString();
                ramSlot.Initialize(fullAddress);

                // Log address just to double check
                Debug.Log($"Instantiated slot with address: {ramSlot.slotAddress}");

                // Rename the slot GameObject
                slot.name = "Slot_" + fullAddress;
            }
        }
    }
}
