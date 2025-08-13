using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
   public static PlayerSpawnManager Instance { get; private set; }

    // Key: scene name -> spawnId to use WHEN that scene loads
    private readonly Dictionary<string, string> sceneSpawnMap =
        new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Set the spawn to use the NEXT TIME the given scene loads.
    /// Example: leaving "Motherboard" via the Mikey portal:
    ///   SetReturnSpawnForScene("Motherboard", "Mikey");
    /// Then when Motherboard loads again, we'll use its "Mikey" spawn.
    /// </summary>
    public void SetReturnSpawnForScene(string sceneNameToReturnTo, string spawnPointIdInThatScene)
    {
        if (string.IsNullOrWhiteSpace(sceneNameToReturnTo))
        {
            Debug.LogError("[SpawnSet] sceneNameToReturnTo is null/empty.");
            return;
        }

        var id = string.IsNullOrWhiteSpace(spawnPointIdInThatScene) ? "Default" : spawnPointIdInThatScene.Trim();
        sceneSpawnMap[sceneNameToReturnTo] = id;
        Debug.Log($"[SpawnSet-Return] '{sceneNameToReturnTo}' <- '{id}'");
    }

    /// <summary>
    /// Optional helper: set a return spawn for the current scene, then load the next scene.
    /// </summary>
    public void LoadSceneAndRememberReturn(string nextSceneName, string returnToCurrentSceneSpawnId)
    {
        var current = SceneManager.GetActiveScene().name;
        SetReturnSpawnForScene(current, returnToCurrentSceneSpawnId);
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PlacePlayerWhenReady(scene));
    }

    private IEnumerator PlacePlayerWhenReady(Scene scene)
    {
        string sceneName = scene.name;

        // Look up the intended spawn for THIS scene
        string spawnId = sceneSpawnMap.TryGetValue(sceneName, out var s) ? s : "Default";
        Debug.Log($"[SpawnLookup] Using spawnId '{spawnId}' for scene '{sceneName}'.");

        // Wait a couple frames so Player + spawn points exist
        yield return null;
        yield return null;

        // Find Player (retry briefly)
        GameObject player = null;
        float timeout = 1.0f;
        float elapsed = 0f;
        while (player == null && elapsed < timeout)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) break;
            yield return null;
            elapsed += Time.unscaledDeltaTime;
        }

        if (player == null)
        {
            Debug.LogWarning($"[Spawn] Player not found in scene '{sceneName}'.");
            yield break;
        }

        // Find spawn points IN THIS SCENE (including inactive)
        var allSpawnPoints = Resources.FindObjectsOfTypeAll<PlayerSpawnPoint>();
        var sceneSpawnPoints = new List<PlayerSpawnPoint>();
        foreach (var sp in allSpawnPoints)
        {
            if (sp != null && sp.gameObject.scene.IsValid() && sp.gameObject.scene == scene)
                sceneSpawnPoints.Add(sp);
        }

        Debug.Log($"[SpawnScan] Found {sceneSpawnPoints.Count} spawn points in scene '{sceneName}'.");

        // Match by id (case-insensitive)
        PlayerSpawnPoint target = null;
        foreach (var sp in sceneSpawnPoints)
        {
            if (sp == null) continue;
            if (string.Equals(sp.spawnId?.Trim(), spawnId, System.StringComparison.OrdinalIgnoreCase))
            {
                target = sp;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogWarning($"[Spawn] No PlayerSpawnPoint with id '{spawnId}' in scene '{sceneName}'. Using whatever the scene sets.");
            yield break;
        }

        player.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);
        Debug.Log($"[Spawn] Placed Player at '{spawnId}' in scene '{sceneName}'.");

        // Optional re-affirm in case another script moves Player on Start
        yield return null;
        player.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);
        Debug.Log($"[Spawn] Re-affirmed Player at '{spawnId}' after one frame.");

        // Optional: clear once used so it doesn't stick forever
        // sceneSpawnMap.Remove(sceneName);
    }
}
