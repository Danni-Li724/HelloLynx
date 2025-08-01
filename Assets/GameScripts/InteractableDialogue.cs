using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InteractableDialogue : MonoBehaviour
{
    public enum InteractableType
    {
        TransitionToScene,
        ShowInfo
    }

    public InteractableType interactableType;
    public string transitionSceneName;
    public string spawnId;
    public string infoDescription;

    private static InteractableDialogue activeInteractable;

    void Start()
    {
        PlayerDetector detector = GetComponent<PlayerDetector>();
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

                if (interactableType == InteractableType.ShowInfo)
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
                case InteractableType.ShowInfo:
                    ShowInfo();
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

    private void ShowInfo()
    {
        InfoPanel.instance.show(infoDescription);
    }
}

