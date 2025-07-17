using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private void OnEnable()
    {
        var inputActions = new PlayerControls();
        inputActions.Enable();
        inputActions.Player.Reaction.performed += ctx => HandleReaction();
    }

    void HandleReaction()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.TryReact();
        }
    }
}