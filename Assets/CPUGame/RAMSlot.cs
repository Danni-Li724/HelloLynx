using UnityEngine;

public class RAMSlot : MonoBehaviour
{
    public string slotAddress; 

    public void Initialize(string address)
    {
        slotAddress = address.ToLower();
    }
}
