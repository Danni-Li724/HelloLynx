using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public enum AnimationState
    {
        IdleFront,
        IdleBack,
        IdleRight,   
        IdleLeft,     
        WalkFront,
        WalkBack,
        WalkRight,
        WalkLeft
    }

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimation(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.IdleFront:
                animator.Play("IdleFront");
                break;
            case AnimationState.IdleBack:
                animator.Play("IdleBack");
                break;
            case AnimationState.IdleRight:  
                animator.Play("IdleRight");
                break;
            case AnimationState.IdleLeft:   
                animator.Play("IdleLeft");
                break;
            case AnimationState.WalkFront:
                animator.Play("WalkFront");
                break;
            case AnimationState.WalkBack:
                animator.Play("WalkBack");
                break;
            case AnimationState.WalkRight:
                animator.Play("WalkRight");
                break;
            case AnimationState.WalkLeft:
                animator.Play("WalkLeft");
                break;
        }
    }
}
