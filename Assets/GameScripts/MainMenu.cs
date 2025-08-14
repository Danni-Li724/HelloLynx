using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [Header("Transforms of UI stuff")] 
    public RectTransform leftText;
    public RectTransform rightText;
    public RectTransform popButton;

    [Header("DOTween settings")] 
    public float textDuration = 1f;
    public float buttonDelay = 0.5f;
    public float buttonDuration = 0.8f;
    public float floatStrength = 10f;
    public float floatDuration = 1.2f;
    public float buttonYOffset = -40f;

    [Header("Pop Image (optional)")]
    [Tooltip("UI element that pops in with a scale animation")]
    public RectTransform popImage;
    [Tooltip("Delay before the pop image scales in (relative to start)")]
    public float imageDelay = 0.5f;
    [Tooltip("How long the pop scale takes")]
    public float imageScaleDuration = 0.6f;
    [Tooltip("Ease for the pop scale animation")]
    public Ease imageScaleEase = Ease.OutBack;

    private Vector2 leftStartPos, rightStartPos, buttonStartPos;
    private Vector2 leftEndPos, rightEndPos, buttonEndPos;
    private Vector3 imageEndScale = Vector3.one;

    void Start()
    {
        // Cache end positions
        leftEndPos = leftText.anchoredPosition;
        rightEndPos = rightText.anchoredPosition;
        buttonEndPos = popButton.anchoredPosition;
        buttonEndPos.y += buttonYOffset;

        // Compute start positions (off-screen)
        leftStartPos = leftEndPos + Vector2.left * (Screen.width + 200);
        rightStartPos = rightEndPos + Vector2.right * (Screen.width + 200);
        buttonStartPos = buttonEndPos + Vector2.up * (Screen.width + 80);

        // Apply start positions
        leftText.anchoredPosition = leftStartPos;
        rightText.anchoredPosition = rightStartPos;
        popButton.anchoredPosition = buttonStartPos;

        // Prep pop image
        if (popImage != null)
        {
            imageEndScale = popImage.localScale;         // remember original
            popImage.localScale = Vector3.zero;          // start invisible (scaled down)
        }

        AnimateUI();
    }

    void AnimateUI()
    {
        // Slide left/right text in
        leftText.DOAnchorPos(leftEndPos, textDuration).SetEase(Ease.OutCubic);
        rightText.DOAnchorPos(rightEndPos, textDuration).SetEase(Ease.OutCubic);

        // Slide button in, then float
        popButton.DOAnchorPos(buttonEndPos, buttonDuration)
            .SetDelay(buttonDelay + textDuration * 0.7f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                popButton
                    .DOAnchorPosY(buttonEndPos.y + floatStrength, floatDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });

        // Pop image scale (lines up to start during the button slide)
        if (popImage != null)
        {
            // Kick off around when the button starts to arrive
            float popStartDelay = buttonDelay + textDuration * 0.5f + imageDelay;
            popImage
                .DOScale(imageEndScale, imageScaleDuration)
                .SetDelay(popStartDelay)
                .SetEase(imageScaleEase);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Motherboard");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
