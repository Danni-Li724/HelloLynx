using UnityEngine;
using System;

public class PlayerDetector : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPlayerEnter?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPlayerExit?.Invoke();
    }
}