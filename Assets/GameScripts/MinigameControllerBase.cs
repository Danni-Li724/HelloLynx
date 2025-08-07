using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class MinigameControllerBase : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public GameObject minigameUIPanel;
    public GameObject minigameGameplay;
    public GameObject startButtonObj;
    public GameObject tryAgainButtonObj;
    public GameObject returnButtonObj;
    public Text resultText;

    [Header("Pop Button")] 
    public GameObject popButton;
    public float popScale = 1.2f;
    public float popDuration = 0.2f;
    public float floatStrength = 20f;
    public float floatDuration = 1.2f;

    private RectTransform rectTransform;
    private Vector2 startAnchoredPos;
    private Vector3 originalScale;

    [Header("Badge Reward")]
    public BadgeType badgeTypeForWin;

    void Awake()
    {
        rectTransform = popButton.GetComponent<RectTransform>();
        startAnchoredPos = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
    }
    protected virtual void Start()
    {
        if (minigameUIPanel) minigameUIPanel.SetActive(false);
        if (minigameGameplay) minigameGameplay.SetActive(false);
        if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
        if (returnButtonObj) returnButtonObj.SetActive(false);
        if (resultText) resultText.text = "";
        
        // dialogue button stuff
        rectTransform.localScale = originalScale;
        rectTransform.DOScale(originalScale * popScale, popDuration * 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                rectTransform.DOScale(originalScale, popDuration * 0.5f)
                    .SetEase(Ease.InBack)
                    .OnComplete(StartFloating);
            });
    }

    public void HidePopButton()
    {
        if (popButton) popButton.SetActive(false);
    }

    void StartFloating()
    {
        rectTransform.DOAnchorPosY(startAnchoredPos.y + floatStrength, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    // Call when dialogue finished
    public virtual void OnDialogueFinished()
    {
        if (startButtonObj) startButtonObj.SetActive(true);
    }

    // Called by Start Minigame button
    public virtual void StartMinigame()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (minigameUIPanel) minigameUIPanel.SetActive(true);
        if (minigameGameplay) minigameGameplay.SetActive(true);

        if (startButtonObj) startButtonObj.SetActive(false);
        if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
        if (returnButtonObj) returnButtonObj.SetActive(false);
        if (resultText) resultText.text = "";

        BeginMinigame(); // Let child actually start the minigame
    }

    // Abstract: each minigame must implement its own start logic (enable timer etc.)
    protected abstract void BeginMinigame();

    // For CPU minigame: call this when game over (timer or logic decides)
    // For Collision minigame: call from controller or minigame
    public virtual void OnMinigameComplete(bool playerWon)
    {
        if (minigameGameplay) minigameGameplay.SetActive(false);
        if (minigameUIPanel) minigameUIPanel.SetActive(true);
        if (resultText) resultText.text = playerWon ? "You Win!" : "You Lose!";
        if (returnButtonObj) returnButtonObj.SetActive(true);

        if (playerWon)
        {
            BadgeInventory.Instance?.EarnBadge(badgeTypeForWin);
            if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
        }
        else
        {
            if (tryAgainButtonObj) tryAgainButtonObj.SetActive(true);
        }
    }
    public virtual void TryAgain()
    {
        if (resultText) resultText.text = "";
        if (returnButtonObj) returnButtonObj.SetActive(false);
        if (tryAgainButtonObj) tryAgainButtonObj.SetActive(false);
        if (minigameGameplay) minigameGameplay.SetActive(true);
        if (minigameUIPanel) minigameUIPanel.SetActive(true);

        ResetMinigame();
        StartMinigame();
    }
    protected abstract void ResetMinigame();

    public virtual void ReturnToHub()
    {
        SceneManager.LoadScene("Motherboard");
    }
}
