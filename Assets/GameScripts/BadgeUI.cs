using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BadgeUI : MonoBehaviour
{
   public GameObject badgePanel;
   public Button badgeButton;
   public Transform badgeListParent;
   public GameObject badgeIconPrefab;

   private void Start()
   {
      badgePanel.SetActive(false);
      badgeButton.onClick.AddListener(OpenBadgePanel);
      BadgeInventory.Instance.OnBadgeEarned += BadgeEarnedEffect;
   }

   void BadgeEarnedEffect(BadgeType badge)
   {
      badgeButton.transform.DOPunchScale(Vector2.one * 0.2f, 0.5f, 10, 1);
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
      {
         Destroy(t.gameObject);
      }
      foreach (var badge in BadgeInventory.Instance.GetBadges())
      {
         var icon = Instantiate(badgeIconPrefab, badgeListParent);
         icon.GetComponentInChildren<Text>().text = badge.ToString(); // displaying name
      }
   }
}
