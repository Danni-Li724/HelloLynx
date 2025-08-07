using UnityEngine;

public class MikeyMinigameController : MinigameControllerBase
{
    public CPUGameManager cpuGameManager;

    protected override void BeginMinigame()
    {
        if (cpuGameManager) cpuGameManager.BeginMinigame(OnCpuGameOver);
    }

    public void BeginCPUGame()
    {
        BeginMinigame();
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

