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
   private void Start()
   {
      badgePanel.SetActive(false);
      badgeButton.onClick.AddListener(OpenBadgePanel);
      BadgeInventory.Instance.OnBadgeEarned += BadgeEarnedEffect;
      RefreshBadgeList();
   }

   void OpenBadgePanel()
   {
      badgePanel.SetActive(true);
      RefreshBadgeList();
      BadgeInventory.Instance.CheckBadges();
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
   void BadgeEarnedEffect(BadgeType badge)
   {
      badgeButton.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 10, 1);
   }
}

