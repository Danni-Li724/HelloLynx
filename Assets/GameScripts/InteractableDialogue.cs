using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InteractableDialogue : MonoBehaviour
{
    public string transitionSceneName;
    public string spawnId;

    private bool isPlayerInRange;

    void Start()
    {
        PlayerDetector detector = GetComponent<PlayerDetector>();
        if (detector != null)
        {
            detector.OnPlayerEnter += () => isPlayerInRange = true;
            detector.OnPlayerExit += () => isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange && PlayerInputHandler.Instance.IsInteractPressed)
        {
            // Set info for returning after dialogue/minigame
            PlayerSpawnManager.Instance.SetLastEntryPoint(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                spawnId
            );
            // Track which NPC we're talking to
            NPCInteractionTracker.Instance.SetUpcomingNPC(gameObject.name);
            UnityEngine.SceneManagement.SceneManager.LoadScene(transitionSceneName);
        }
    }
}

