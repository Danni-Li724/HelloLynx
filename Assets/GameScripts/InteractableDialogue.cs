using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InteractableDialogue : MonoBehaviour
{
    public enum InteractableType
    {
        TransitionToScene,
        ShowScreenInfo,
        ShowRamInfo,
        ShowSpeakerInfo,
        ShowPowerInfo
    }

    [Header("UI Settings")] 
    public Color unvistedColor = Color.yellow;
    public Color visitedColor = Color.green;
    private FloatingUI floatingUI;

    public InteractableType interactableType;
    public string transitionSceneName;
    public string spawnId;
    public string infoDescription;
    private bool hasBeenInteracted = false;

    private static InteractableDialogue activeInteractable;

    void Start()
    {
        PlayerDetector detector = GetComponent<PlayerDetector>();
        floatingUI = GetComponent<FloatingUI>();
        if(floatingUI != null) floatingUI.SetColor(unvistedColor);
        if (detector != null)
        {
            detector.OnPlayerEnter += () =>
            {
                activeInteractable = this;
            };
            detector.OnPlayerExit += () =>
            {
                if (activeInteractable == this)
                    activeInteractable = null;

                if (interactableType == InteractableType.ShowScreenInfo || 
                    interactableType == InteractableType.ShowSpeakerInfo ||
                    interactableType == InteractableType.ShowPowerInfo ||
                    interactableType == InteractableType.ShowRamInfo)
                    InfoPanel.instance.hide();
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
            }
        }
    }

    private void TransitionToScene()
    {
        PlayerSpawnManager.Instance.SetLastEntryPoint(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            spawnId
        );
        Debug.Log(spawnId);
        NPCInteractionTracker.Instance.SetUpcomingNPC(gameObject.name);
        UnityEngine.SceneManagement.SceneManager.LoadScene(transitionSceneName);
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

