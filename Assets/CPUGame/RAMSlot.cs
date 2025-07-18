using UnityEngine;

public class RAMSlot : MonoBehaviour
{
    [System.NonSerialized]
    public string slotAddress; 

    public void Initialize(string address)
    {
        slotAddress = address.ToLower();
    }
}
