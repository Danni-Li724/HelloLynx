using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float gameDuration = 30f;
    public Text timerText;
    public Text resultText;

    private float elapsedTime = 0f;
    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded) return;

        elapsedTime += Time.deltaTime;
        float remaining = Mathf.Clamp(gameDuration - elapsedTime, 0, gameDuration);
        timerText.text = $"Time: {remaining:F1}s";

        if (elapsedTime >= gameDuration)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;

        if (CollisionManager.Instance.PlayerWon())
        {
            resultText.text = "You Win!";
        }
        else
        {
            resultText.text = "You Lose!";
        }
    }
}

