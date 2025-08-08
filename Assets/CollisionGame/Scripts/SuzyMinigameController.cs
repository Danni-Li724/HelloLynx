using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SuzyMinigameController : MinigameControllerBase
{
    public CollisionManager collisionManager;
    public GameTimer gameTimer;
    public GameObject startButton;
    public GameObject gameStartPanel;

    private void Awake()
    {
        HookStartButtonToDialogue();
    }

    private void HookStartButtonToDialogue()
    {
        if (startButton == null) return;
        DialogueTest dialogueTest = FindObjectOfType<DialogueTest>();
        if (dialogueTest == null) return;
        Button button = startButton.GetComponent<Button>();
        if(button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(dialogueTest.SuzyTestPressed);
        }
    }

    protected override void BeginMinigame()
    {
        if (collisionManager) collisionManager.BeginMinigame();
        if (gameTimer) {
            gameTimer.OnTimerEnd = OnGameTimerEnd;
            gameTimer.StartTimer();
        }
    }

    public void BeginCollisionGame()
    {
        startButton.SetActive(true);
        gameStartPanel.SetActive(false);
    }
    protected override void ResetMinigame()
    {
        if (collisionManager) collisionManager.ResetMinigame();
        if (gameTimer) gameTimer.ResetTimer();
    }

    // Call this when timer is done (assigned above)
    private void OnGameTimerEnd()
    {
        if (collisionManager) collisionManager.FinishMinigame(); // calls OnMinigameComplete(bool)
    }
}

