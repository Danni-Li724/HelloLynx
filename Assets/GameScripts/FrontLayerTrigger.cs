using UnityEngine;
using System.Collections.Generic;

public class FrontLayerTrigger : MonoBehaviour
{
    public string dockFrontLayerName = "DockFront";
    private int dockFrontSortingOrder = 10;

    private class OriginalSorting
    {
        public string layerName;
        public int order;
    }
    private Dictionary<Renderer, OriginalSorting> originalSorting = new Dictionary<Renderer, OriginalSorting>();
    void OnTriggerStay2D(Collider2D other)
    {
        GameObject rootObj = other.transform.root.gameObject;
        if (rootObj.CompareTag("Player") || rootObj.CompareTag("Byte"))
        { 
           Renderer renderer = rootObj.GetComponentInChildren<Renderer>();
           if (renderer == null) return;
           if (!originalSorting.ContainsKey(renderer))
           {
               originalSorting[renderer] = new OriginalSorting()
               {
                   layerName = renderer.sortingLayerName,
                   order = renderer.sortingOrder
               };
           }
           renderer.sortingLayerName = dockFrontLayerName;
           renderer.sortingOrder = dockFrontSortingOrder;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        GameObject rootObj = other.transform.root.gameObject;
        Renderer renderer = rootObj.GetComponentInChildren<Renderer>();
        if (renderer == null) return;
        if (originalSorting.TryGetValue(renderer, out var info))
        {
            renderer.sortingLayerName = info.layerName;
            renderer.sortingOrder = info.order;
            originalSorting.Remove(renderer);
        }
    }
}
