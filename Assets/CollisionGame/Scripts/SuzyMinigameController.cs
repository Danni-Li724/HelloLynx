using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SuzyMinigameController : MinigameControllerBase
{
    public CollisionManager collisionManager;
    public GameTimer gameTimer;

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
        BeginMinigame();
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

