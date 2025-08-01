using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BadgeUI : MonoBehaviour
{
   public GameObject badgePanel;
    public Button badgeButton;
    public Transform badgeListParent;
    public GameObject badgeIconPrefab;
    public BadgeSpriteLibrary badgeSpriteLibrary;

    private bool badgePanelOpen = false;
    private Tween punchTween;
    private Tween colorTween;
    private Color originalColor;

    private void Start()
    {
        badgePanel.SetActive(false);
        badgePanelOpen = false;
        badgeButton.onClick.AddListener(ToggleBadgePanel);
        BadgeInventory.Instance.OnBadgeEarned += StartEarnedEffect;
        BadgeInventory.Instance.OnBadgeChecked += StopEarnedEffect;

        // Save the original button color
        originalColor = badgeButton.image.color;

        // If there's an unchecked badge, start looping the effect!
        if (BadgeInventory.Instance.HasnewBadge())
            StartEarnedEffect(default);

        RefreshBadgeList();
    }

    void ToggleBadgePanel()
    {
        badgePanelOpen = !badgePanelOpen;
        badgePanel.SetActive(badgePanelOpen);

        if (badgePanelOpen)
        {
            RefreshBadgeList();
            BadgeInventory.Instance.CheckBadges(); // This will call StopEarnedEffect
        }
    }

    void RefreshBadgeList()
    {
        foreach (Transform t in badgeListParent)
            Destroy(t.gameObject);

        foreach (var badge in BadgeInventory.Instance.GetBadges())
        {
            var icon = Instantiate(badgeIconPrefab, badgeListParent);
            var image = icon.GetComponentInChildren<Image>();
            var text = icon.GetComponentInChildren<Text>();
            if (image) image.sprite = badgeSpriteLibrary.GetSprite(badge);
            if (text) text.text = badge.ToString();
        }
    }

    void StartEarnedEffect(BadgeType badge)
    {
        StopEarnedEffect();
        // Loop punch scale
        punchTween = badgeButton.transform
            .DOPunchScale(Vector3.one * 0.2f, 0.5f, 10, 1)
            .SetLoops(-1, LoopType.Restart);
        // Loop color flash
        colorTween = badgeButton.image
            .DOColor(Color.green, 0.3f)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void StopEarnedEffect()
    {
        punchTween?.Kill();
        colorTween?.Kill();
        badgeButton.image.color = originalColor;
        badgeButton.transform.localScale = Vector3.one;
    }
}

