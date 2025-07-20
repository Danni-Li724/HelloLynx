using UnityEngine;
using System;

public class DialogueEvents : MonoBehaviour
{
    public event Action<string> onEnterDialogue;

    public void EnterDialogue(string knotName)
    {
        onEnterDialogue?.Invoke(knotName);
    }
}
