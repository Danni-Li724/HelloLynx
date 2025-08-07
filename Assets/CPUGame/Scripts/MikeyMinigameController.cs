using UnityEngine;
using UnityEngine.UI;

public class MikeyMinigameController : MinigameControllerBase
{
    public CPUGameManager cpuGameManager;
    public GameObject startButton;
    public GameObject gameStartPanel;
    
    private void OnEnable()
    {
        HookStartButtonToDialogue();
    }

    private void HookStartButtonToDialogue()
    {
        if (startButton == null) return;
        DialogueTest dialogueTest = FindAnyObjectByType<DialogueTest>();
        if (dialogueTest == null) return;
        Button button = startButton.GetComponent<Button>();
        if(button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(dialogueTest.MikeyTestPressed);
        }
    }
    protected override void BeginMinigame()
    {
        if (cpuGameManager) cpuGameManager.BeginMinigame(OnCpuGameOver);
    }

    public void BeginCPUGame()
    {
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

