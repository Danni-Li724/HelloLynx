using UnityEngine;
using UnityEngine.UI;

public class MikeyMinigameController : MinigameControllerBase
{
    public CPUGameManager cpuGameManager;
    public GameObject startButton;
    public GameObject gameStartPanel;
    
    private void Awake()
    {
        HookStartButtonToDialogue();
        //AudioManager.Instance.StopBackgroundMusic();
        AudioManager.Instance.PlayCPUMusic();
    }

    private void HookStartButtonToDialogue()
    {
        if (startButton == null) return;

        // Look for the GameObject by exact name
        GameObject go = GameObject.Find("DialogueTest");
        if (go == null)
        {
            Debug.LogWarning("SuzyMinigameController: No GameObject named 'DialogueTest' found in scene.");
            return;
        }

        // Try to get the component
        DialogueTest dialogueTest = go.GetComponent<DialogueTest>();
        if (dialogueTest == null)
        {
            Debug.LogWarning("SuzyMinigameController: GameObject 'DialogueTest' found, but no DialogueTest component attached.");
            return;
        }

        // Hook the button
        Button button = startButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(dialogueTest.MikeyTestPressed);
        }
    }
    protected override void BeginMinigame()
    {
        if (cpuGameManager) cpuGameManager.BeginMinigame(OnCpuGameOver);
    }

    public void ShowStartDialogueButton()
    {
        if (popButton) popButton.SetActive(true);
    }

    public void BeginCPUGame()
    {
        NPCInteractionTracker.Instance?.MarkVisited("Mikey");
        startButton.SetActive(true);
        gameStartPanel.SetActive(false);
    }

    protected override void ResetMinigame()
    {
        if (cpuGameManager) cpuGameManager.ResetMinigame();
    }

    // The CPU game manager should call this on game over:
    private void OnCpuGameOver(bool playerWon)
    {
        OnMinigameComplete(playerWon);
    }
}

