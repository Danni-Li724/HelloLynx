using UnityEngine;
using System.Collections.Generic;

public class YSortManager : MonoBehaviour
{
    [Tooltip("List of GameObjects to Y-sort every frame")]
    public List<GameObject> objectsToSort = new List<GameObject>();

    private void LateUpdate()
    {
        foreach (var obj in objectsToSort)
        {
            if (obj == null) continue;

            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                YSortHelper.ApplyYSort(sr, obj.transform);
            }
        }
    }
}
