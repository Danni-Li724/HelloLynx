using UnityEngine;
using UnityEngine.UI;

public class InteractableDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    private bool isPlayerInRange;

    private void Start()
    {
        PlayerDetector detector = GetComponent<PlayerDetector>();
        if (detector != null)
        {
            detector.OnPlayerEnter += () => isPlayerInRange = true;
            detector.OnPlayerExit += () => {
                isPlayerInRange = false;
                dialoguePanel.SetActive(false);
            };
        }
    }

    private void Update()
    {
        if (isPlayerInRange && PlayerInputHandler.Instance.IsInteractPressed)
        {
            dialoguePanel.SetActive(true);
        }
    }
}

