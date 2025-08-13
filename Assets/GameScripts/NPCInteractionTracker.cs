using UnityEngine;
using System.Collections.Generic;
using System;

public class NPCInteractionTracker : MonoBehaviour
{
    public static NPCInteractionTracker Instance { get; private set; }
    private readonly HashSet<string> visited = new HashSet<string>();

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MarkVisited(string npcKey)
    {
        if (string.IsNullOrWhiteSpace(npcKey)) return;
        visited.Add(npcKey);
    }

    public bool HasVisited(string npcKey)
    {
        return !string.IsNullOrWhiteSpace(npcKey) && visited.Contains(npcKey);
    }
}

/// Example Code for you Jamie:
/* public class NPC : Monobehaviour
{
    public string npcId;

    public void Interact()
    {
        NPCInteractionTracker.Instance.TalkTo(npcId);
        // do your dialogue stuff here
        if (NPCInteractionTracker.Instance.HasTalkedTo("NPCname"))
        {
            // special implementations/dialogues
        }
    }
}*/ 

