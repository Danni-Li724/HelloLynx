using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : AutomaticMovement
{
    [Header("Player Settings")]
    public bool allowManualControl = true;
    public bool shouldFaceDirection = true;

    private Vector2 lastInputDirection = Vector2.zero;
    private Rigidbody2D rb;
    private CharacterAnimationController animationController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<CharacterAnimationController>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing! Please add it to the player.");
        }
    }

    private void FixedUpdate()
    {
        if (!allowManualControl) return;

        Vector2 input = PlayerInputHandler.Instance?.MovementInput ?? Vector2.zero;

        if (input != Vector2.zero)
        {
            MovePlayer(input);
            lastInputDirection = input;
            AudioManager.Instance.PlayFootsteps();
            if (shouldFaceDirection)
            {
                FaceDirection(input);
            }

            UpdateAnimationState(input);
        }
        else
        {
            SetIdleAnimation(lastInputDirection);
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void DisableManualControl()
    {
        allowManualControl = false;
    }

    public void EnableManualControl()
    {
        allowManualControl = true;
    }

    private void SetIdleAnimation(Vector2 direction)
    {
        AudioManager.Instance?.footstepSource?.Stop();
        if (direction.x > 0)
            animationController.SetAnimation(CharacterAnimationController.AnimationState.IdleRight);
        else if (direction.x < 0)
            animationController.SetAnimation(CharacterAnimationController.AnimationState.IdleLeft);
        else if (direction.y < 0)
            animationController.SetAnimation(CharacterAnimationController.AnimationState.IdleFront);
        else if (direction.y > 0)
            animationController.SetAnimation(CharacterAnimationController.AnimationState.IdleBack);
    }

    private void UpdateAnimationState(Vector2 direction)
    {
        if (direction.magnitude == 0)
        {
            SetIdleAnimation(direction);
        }
        else
        {
            if (direction.y < 0)
                animationController.SetAnimation(CharacterAnimationController.AnimationState.WalkFront);
            else if (direction.y > 0)
                animationController.SetAnimation(CharacterAnimationController.AnimationState.WalkBack);
            else if (direction.x > 0)
                animationController.SetAnimation(CharacterAnimationController.AnimationState.WalkRight);
            else if (direction.x < 0)
                animationController.SetAnimation(CharacterAnimationController.AnimationState.WalkLeft);
        }
    }
}
