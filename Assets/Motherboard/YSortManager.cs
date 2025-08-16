using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class YSortManager : MonoBehaviour
{
   [Tooltip("List of GameObjects to Y-sort every frame")]
    public List<GameObject> objectsToSort = new List<GameObject>();

    [Header("Auto-track PlayerView")]
    [SerializeField] private bool autoTrackPlayerView = true;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string playerViewChildName = "PlayerView";
    [SerializeField] private float recheckInterval = 0.5f;

    private float _nextRecheckTime;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Try immediately in case Player already exists in the persistent scene
        EnsurePlayerViewTracked();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // New scene loaded: re-ensure PlayerView is tracked
        EnsurePlayerViewTracked();
    }

    private void LateUpdate()
    {
        // Keep things tidy (remove nulls)
        CleanupNulls();

        // Periodic recheck to catch late spawns or hierarchy changes
        if (autoTrackPlayerView && Time.time >= _nextRecheckTime)
        {
            EnsurePlayerViewTracked();
            _nextRecheckTime = Time.time + recheckInterval;
        }

        // Apply Y-sort
        for (int i = 0; i < objectsToSort.Count; i++)
        {
            var obj = objectsToSort[i];
            if (obj == null) continue;

            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                YSortHelper.ApplyYSort(sr, obj.transform);
            }
        }
    }

    private void EnsurePlayerViewTracked()
    {
        if (!autoTrackPlayerView) return;

        // If we already have a live PlayerView in the list, we’re good
        if (HasLivePlayerViewTracked()) return;

        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null) return;

        // Find child named "PlayerView" (search inactive too)
        Transform[] allChildren = player.GetComponentsInChildren<Transform>(true);
        Transform found = null;
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].name == playerViewChildName)
            {
                found = allChildren[i];
                break;
            }
        }
        if (found == null) return;

        // Add if not already present
        AddToSortListIfNeeded(found.gameObject);
    }

    private bool HasLivePlayerViewTracked()
    {
        // Check if list currently contains a non-null GO named PlayerView
        for (int i = 0; i < objectsToSort.Count; i++)
        {
            GameObject go = objectsToSort[i];
            if (go != null && go.name == playerViewChildName) return true;
        }
        return false;
    }

    private void AddToSortListIfNeeded(GameObject go)
    {
        if (go == null) return;
        for (int i = 0; i < objectsToSort.Count; i++)
        {
            if (objectsToSort[i] == go) return; // already tracked
        }
        objectsToSort.Add(go);
    }

    private void CleanupNulls()
    {
        // Remove null entries safely (iterate backwards)
        for (int i = objectsToSort.Count - 1; i >= 0; i--)
        {
            if (objectsToSort[i] == null)
            {
                objectsToSort.RemoveAt(i);
            }
        }
    }
}
