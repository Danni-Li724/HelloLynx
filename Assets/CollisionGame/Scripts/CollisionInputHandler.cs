using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionInputHandler : MonoBehaviour
{
    private CollisionControls _input; 

    private void Awake()
    {
        _input = new CollisionControls();      
    }

    private void OnEnable()
    {
        _input.Player.Reaction.performed += OnReaction; 
        _input.Player.Enable();                        
    }

    private void OnDisable()
    {
        _input.Player.Reaction.performed -= OnReaction; 
        _input.Player.Disable();                        
    }

    private void OnDestroy()
    {
        _input.Dispose(); 
    }

    private void OnReaction(InputAction.CallbackContext ctx)
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.TryReact();
    }
}