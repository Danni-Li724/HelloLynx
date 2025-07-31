using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public DialogueEvents dialogueEvents;

    private void Awake()
    {
        Debug.Log("GameEventsManager.Awake");
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); 
        dialogueEvents = new DialogueEvents();
    }
}

