using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class InteractableDialogue : MonoBehaviour
{
    public enum InteractableType
    {
        TransitionToScene,
        ShowScreenInfo,
        ShowRamInfo,
        ShowSpeakerInfo,
        ShowPowerInfo,
        TouchCapacitor,
        TouchMinion
    }

    [Header("UI Settings")] 
    public Color unvistedColor = Color.yellow;
    public Color visitedColor = Color.white;
    private FloatingUI floatingUI;
    
    [Header("Interactable Settings")]
    public Capacitor targetCapacitor; 
    public bool restartIfAlreadySpawning = false;
    public InteractableType interactableType;
    public string transitionSceneName;
    public string returnSpawnIdForThisScene;
    public string spawnId;
    private bool hasBeenInteracted = false;
    public GameObject dialoguePanel;
    public GameObject minionPanelPrefab;
    public string minionDescription;
    
    [Header("NPC")]
    [SerializeField] private string npcKey;


    private static InteractableDialogue activeInteractable;

    void Start()
    {
        PlayerDetector detector = GetComponent<PlayerDetector>();
        dialoguePanel = GameObject.Find("DialoguePanelUI");
        floatingUI = GetComponent<FloatingUI>();
        if(floatingUI != null) floatingUI.SetColor(unvistedColor);
        if (detector != null)
        {
            detector.OnPlayerEnter += () =>
            {
                activeInteractable = this;
                // if (interactableType == InteractableType.ShowScreenInfo || 
                //     interactableType == InteractableType.ShowSpeakerInfo ||
                //     interactableType == InteractableType.ShowPowerInfo ||
                //     interactableType == InteractableType.ShowRamInfo)
                // {if(dialoguePanel) dialoguePanel.SetActive(true);}
            };
            detector.OnPlayerExit += () =>
            {
                if (activeInteractable == this)
                    activeInteractable = null;

                // if (interactableType == InteractableType.ShowScreenInfo ||
                //     interactableType == InteractableType.ShowSpeakerInfo ||
                //     interactableType == InteractableType.ShowPowerInfo ||
                //     interactableType == InteractableType.ShowRamInfo)
                // {
                //     if(dialoguePanel) dialoguePanel.SetActive(false);
                // }
            };
        }
    }

    void Update()
    {
        if (activeInteractable == this && PlayerInputHandler.Instance.IsInteractPressed)
        {
            switch (interactableType)
            {
                case InteractableType.TransitionToScene:
                    TransitionToScene();
                    break;
                case InteractableType.ShowScreenInfo:
                    ShowScreenInfo();
                    break;
                case InteractableType.ShowRamInfo:
                    ShowRAMInfo();
                    break;
                case InteractableType.ShowSpeakerInfo:
                    ShowSpeakerInfo();
                    break;
                case InteractableType.ShowPowerInfo:
                    ShowPowerInfo();
                    break;
                case InteractableType.TouchCapacitor:
                    TouchCapacitorAction();
                    break;
                case InteractableType.TouchMinion:
                    TouchMinionAction();
                    break;
            }
        }
    }

    private void TouchMinionAction()
    {
        StartCoroutine(ShowMinionPanel());
    }

    private IEnumerator ShowMinionPanel()
    {
        Vector3 offset = Vector3.up * 13f;
       GameObject minionPanel = Instantiate(minionPanelPrefab, transform.position, Quaternion.identity);
       Text minionText = minionPanel.GetComponentInChildren<Text>();
       minionText.text = minionDescription;
       minionPanel.transform.SetParent(this.transform);
       minionPanel.transform.position = this.transform.position + offset;
       yield return new WaitForSeconds(8f);
       Destroy(minionPanel);
    }

    private void TransitionToScene()
    {
        if (string.IsNullOrWhiteSpace(transitionSceneName))
        {
            Debug.LogError("[Transition] transitionSceneName is empty.");
            return;
        }

        // We’re leaving the CURRENT scene. Tell the manager which spawn to use
        // when we RETURN to this scene later.
        var current = SceneManager.GetActiveScene().name;
        PlayerSpawnManager.Instance.SetReturnSpawnForScene(current, returnSpawnIdForThisScene);

        Debug.Log($"[Transition] Leaving '{current}' via '{returnSpawnIdForThisScene}' return marker, loading '{transitionSceneName}'.");

        SceneManager.LoadScene(transitionSceneName);
    }

    private void ShowScreenInfo()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue("screenTrivia.Introduction");
        MarkAsVisited();
    }

    private void ShowRAMInfo()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue("ramTrivia.Introduction");
        MarkAsVisited();
    }

    private void ShowSpeakerInfo()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue("speakerTrivia.Introduction");
        MarkAsVisited();
    }

    private void ShowPowerInfo()
    {
        GameEventsManager.instance.dialogueEvents.EnterDialogue("powerTrivia.Introduction");
        MarkAsVisited();
    }
    
    private void TouchCapacitorAction()
    {
        StartCoroutine(ShowMinionPanel());
        if (!targetCapacitor)
        {
            return;
        }

        if (restartIfAlreadySpawning)
            targetCapacitor.ResetAndStart();
        else if (!targetCapacitor.IsSpawning)
            targetCapacitor.StartSpawning();

        MarkAsVisited();
        // if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    private void MarkAsVisited()
    {
        if (!hasBeenInteracted)
        {
            hasBeenInteracted = true;
            if(floatingUI != null)
                floatingUI.SetColor(visitedColor);
        }
    }
}

