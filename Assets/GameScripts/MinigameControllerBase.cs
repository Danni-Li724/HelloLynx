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

    protected virtual void Awake()
    {
        // DO NOT stomp inspector refs with Find() here
        SceneManager.sceneLoaded += OnSceneLoaded;

        InitPopButton();   // safe even if popButton is assigned now
    }

    protected virtual void OnEnable()
    {
        // If arriving from another scene, refs might be missing -> rebind lazily
        RebindIfMissing();
    }

    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        // New scene loaded: rebind scene refs if they’re missing
        RebindIfMissing();
        InitPopButton(); // popButton might be different per scene
    }

    private void RebindIfMissing()
    {
        // Only fill if NULL; never overwrite valid inspector links
        if (!dialoguePanel) dialoguePanel = FindByNameInActiveScene("UI Canvas"); // match your actual name
        if (!startButtonObj) startButtonObj = FindByNameInActiveScene("DialogueStartButton");
        // Add other lookups if you truly need them.
    }

    private GameObject FindByNameInActiveScene(string name)
    {
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            var t = roots[i].transform;
            var hit = FindRecursive(t, name);
            if (hit) return hit.gameObject;
        }
        return null;
    }

    private Transform FindRecursive(Transform t, string name)
    {
        if (t.name == name) return t;
        for (int i = 0; i < t.childCount; i++)
        {
            var r = FindRecursive(t.GetChild(i), name);
            if (r) return r;
        }
        return null;
    }

    private void InitPopButton()
    {
        if (!popButton) { rectTransform = null; return; }

        rectTransform = popButton.GetComponent<RectTransform>();
        if (!rectTransform)
        {
            Debug.LogError($"[{name}] popButton is not a UI element (missing RectTransform).");
            return;
        }

        startAnchoredPos = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
    }

    protected virtual void Start()
    {
        RebindIfMissing();
        InitPopButton();
        
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
