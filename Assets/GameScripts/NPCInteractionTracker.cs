using UnityEngine;
using System.Collections.Generic;
using System;

public class NPCInteractionTracker : MonoBehaviour
{
    public static NPCInteractionTracker Instance { get; private set; }
    private HashSet<string> talkedToNpcs = new HashSet<string>();
    
    // events for jamie
    public event Action<string> OnNpcTalkedTo;
    public event Action<string> ONNpcFirstTime;

    void Awake()
    {
        if(Instance && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void TalkTo(string npcId)
    {
        bool firstTime = talkedToNpcs.Add(npcId);
        OnNpcTalkedTo?.Invoke(npcId);
        if(firstTime)
            ONNpcFirstTime?.Invoke(npcId);
    }
    
    public bool HasTalkedTo(string npcId) => talkedToNpcs.Contains(npcId);
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

