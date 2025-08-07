using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [Header("Transforms of UI stuff")] 
    public RectTransform leftText;
    public RectTransform rightText;
    public RectTransform popButton;

    [Header("Dotween settings")] 
    public float textDuration = 1f;
    public float buttonDelay = 0.5f;
    public float buttonDuration = 0.8f;
    public float floatStrength = 10f;
    public float floatDuration = 1.2f;
    public float buttonYOffset = -40f;

    private Vector2 leftStartPos, rightStartPos, buttonStartPos;
    private Vector2 leftEndPos, rightEndPos, buttonEndPos;

    void Start()
    {
        leftEndPos = leftText.anchoredPosition;
        rightEndPos = rightText.anchoredPosition;
        buttonEndPos = popButton.anchoredPosition;
        buttonEndPos.y += buttonYOffset;

        leftStartPos = leftEndPos + Vector2.left * (Screen.width + 200);
        rightStartPos = rightEndPos + Vector2.right * (Screen.width + 200);
        buttonStartPos = buttonEndPos + Vector2.up * (Screen.width + 80);
        
        leftText.anchoredPosition = leftStartPos;
        rightText.anchoredPosition = rightStartPos;
        popButton.anchoredPosition = buttonStartPos;

        AnimateUI();
    }

    void AnimateUI()
    {
        leftText.DOAnchorPos(leftEndPos, textDuration).SetEase(Ease.OutCubic);
        rightText.DOAnchorPos(rightEndPos, textDuration).SetEase(Ease.OutCubic);

        popButton.DOAnchorPos(buttonEndPos, buttonDuration)
            .SetDelay(buttonDelay + textDuration * 0.7f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                popButton.DOAnchorPosY(buttonEndPos.y + floatStrength, floatDuration).SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Motherboard");
    }
}
