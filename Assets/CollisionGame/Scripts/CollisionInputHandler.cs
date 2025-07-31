using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionInputHandler : MonoBehaviour
{
    private void OnEnable()
    {
        var inputActions = new CollisionControls();
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