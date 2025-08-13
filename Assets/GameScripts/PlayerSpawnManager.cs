using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }
    private Dictionary<string, string> lastEntryPoints = new(); // scenename = spawnpointID?

    void Awake()
    {
        if(Instance && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetLastEntryPoint(string sceneName, string spawnPointId)
    {
        lastEntryPoints[sceneName] = spawnPointId;
        Debug.Log(lastEntryPoints[sceneName]);
        Debug.Log(spawnPointId);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var spawnPoints = GameObject.FindObjectsByType<PlayerSpawnPoint>(FindObjectsSortMode.None);
        string sceneName = scene.name;
        string spawnId = lastEntryPoints.TryGetValue(sceneName, out var s) ? s: "Default";
        foreach (var sp in spawnPoints)
        {
            if (sp.spawnId == spawnId)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if(player != null)
                    player.transform.position = sp.transform.position;
                break;
            }
        }
    }
}
