using Unity.Cinemachine;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    public string playerTag = "Player";
    public bool updateContinuously = false;
    
    public CinemachineCamera vcam;

    private void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    private void OnEnable()
    {
        AssignFollowTarget();
    }

    private void AssignFollowTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            vcam.Follow = player.transform;
        }
    }
}
