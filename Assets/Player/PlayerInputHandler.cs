using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }

    [Header("Input Actions")]
    public InputAction interactAction;
    public InputAction movementAction;

    public Vector2 MovementInput { get; private set; }
    public bool IsInteractPressed { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.Enable();
            interactAction.performed += OnInteractPerformed;
            interactAction.canceled += OnInteractCanceled;
        }

        if (movementAction != null)
        {
            movementAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.performed -= OnInteractPerformed;
            interactAction.canceled -= OnInteractCanceled;
            interactAction.Disable();
        }

        if (movementAction != null)
        {
            movementAction.Disable();
        }
    }

    private void Update()
    {
        // Read movement every frame
        if (movementAction != null)
        {
            MovementInput = movementAction.ReadValue<Vector2>().normalized;
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        IsInteractPressed = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        IsInteractPressed = false;
    }
}