using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float gameDuration = 30f;
    public Text timerText;
    public Text resultText;

    private float elapsedTime = 0f;
    private bool gameEnded = false;
    private bool gameActive = false;

    public System.Action OnTimerEnd; // Callback for MinigameController

    void Update()
    {
        if (!gameActive || gameEnded) return;

        elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp(gameDuration - elapsedTime, 0, gameDuration);
        if (timerText) timerText.text = $"Time: {remaining:F1}s";

        if (elapsedTime >= gameDuration)
        {
            EndGame();
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        gameEnded = false;
        gameActive = true;
        if (timerText) timerText.text = $"Time: {gameDuration:F1}s";
        if (resultText) resultText.text = "";
    }

    public void StopTimer()
    {
        gameActive = false;
    }

    void EndGame()
    {
        gameEnded = true;
        gameActive = false;

        if (OnTimerEnd != null)
            OnTimerEnd.Invoke();
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        gameEnded = false;
        gameActive = false;
        if (timerText) timerText.text = $"Time: {gameDuration:F1}s";
        if (resultText) resultText.text = "";
    }
}

