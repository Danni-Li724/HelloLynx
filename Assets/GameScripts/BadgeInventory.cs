using UnityEngine;
using System.Collections.Generic;
using System;

public enum BadgeType {Suzy, Mikey, Ram, Screen, Audio, Power}

public class BadgeInventory : MonoBehaviour
{
  public static BadgeInventory Instance { get; private set; }
  private HashSet<BadgeType> earnedBadges = new HashSet<BadgeType>();
  private BadgeType? newBadge = null;
  
  public event Action<BadgeType> OnBadgeEarned;
  public event Action OnBadgeChecked;

  void Awake()
  {
    if (Instance && Instance != this) Destroy(gameObject);
    else
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  public void EarnBadge(BadgeType badge)
  {
    if (earnedBadges.Add(badge))
    {
      newBadge = badge;
      OnBadgeEarned?.Invoke(badge);
    }
  }
  
  public bool HasBadge(BadgeType badge) => earnedBadges.Contains(badge);
  public bool HasnewBadge() => newBadge.HasValue;
  
  public IEnumerable<BadgeType> GetBadges() => earnedBadges;

  public void CheckBadges()
  {
    newBadge = null;
    OnBadgeChecked?.Invoke();
  }
}
