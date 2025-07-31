using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigameController : MonoBehaviour
{
    [Header("Minigame Components")]
    public CollisionManager collisionManager;
    public GameTimer gameTimer;

    [Header("UI")]
    public GameObject dialoguePanel;
    public GameObject minigameUIPanel;
    public GameObject minigameGameplay;
    public GameObject startButtonObj;
    public GameObject tryAgainButtonObj;
    public GameObject returnButtonObj;

    [Header("Badge")]
    public BadgeType badgeTypeForWin = BadgeType.Suzy; // Assign per minigame if needed

    private void Start()
    {
        if (minigameUIPanel) minigameUIPanel.SetActive(false);
        if (minigameGameplay) minigameGameplay.SetActive(false);
        if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
        if (returnButtonObj) returnButtonObj.SetActive(false);
        // if (startButtonObj) startButtonObj.SetActive(false); //show after dialogue

        if (gameTimer != null)
            gameTimer.OnTimerEnd = OnGameTimerEnd;
    }

    // Call this from DialogueUI when dialogue finishes
    public void OnDialogueFinished()
    {
        if (startButtonObj) startButtonObj.SetActive(true);
    }

    // Called by Start Minigame button
    public void StartMinigame()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (minigameUIPanel) minigameUIPanel.SetActive(true);
        if (minigameGameplay) minigameGameplay.SetActive(true);

        if (startButtonObj) startButtonObj.SetActive(false);
        if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
        if (returnButtonObj) returnButtonObj.SetActive(false);

        if (collisionManager) collisionManager.BeginMinigame();
        if (gameTimer) gameTimer.StartTimer();
    }

    // This is called by the GameTimer when the timer ends
    public void OnGameTimerEnd()
    {
        if (gameTimer) gameTimer.StopTimer();
        if (collisionManager) collisionManager.FinishMinigame();
        // Don't show results here, let OnMinigameComplete handle UI
    }

    // Called by CollisionManager.FinishMinigame()
    public void OnMinigameComplete(bool playerWon)
    {
        if (minigameGameplay) minigameGameplay.SetActive(false);
        if (minigameUIPanel) minigameUIPanel.SetActive(true); // Timer/results still visible

        if (gameTimer && gameTimer.resultText)
            gameTimer.resultText.text = playerWon ? "You Win!" : "You Lose!";

        if (playerWon)
        {
            BadgeInventory.Instance?.EarnBadge(badgeTypeForWin);
            if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
            if (returnButtonObj) returnButtonObj.SetActive(true);
        }
        else
        {
            if (tryAgainButtonObj) tryAgainButtonObj.SetActive(true);
            if (returnButtonObj) returnButtonObj.SetActive(true);
        }
    }

    public void TryAgain()
    {
        if (collisionManager) collisionManager.ResetMinigame();
        if (gameTimer) gameTimer.ResetTimer();

        if (minigameGameplay) minigameGameplay.SetActive(true);
        if (minigameUIPanel) minigameUIPanel.SetActive(true);

        if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
        if (returnButtonObj) returnButtonObj.SetActive(false);

        StartMinigame(); // Re-start the minigame
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene("Motherboard");
    }
}

