using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : CoreComponent
{
    private FiniteStateMachine stateMachine
    { get => _stateMachine ?? core.GetCoreComponent(ref _stateMachine); }
    private FiniteStateMachine _stateMachine;
    
    private Transform facingDirectionIndicator;
    private Animator animator;

    //Unity Awake
    protected override void Awake()
    {
        base.Awake();

        SetupElements();
    }

    //Setup component elements
    private void SetupElements()
    {
        animator = GetComponent<Animator>();

        facingDirectionIndicator = transform.Find("FacingDirectionIndicator");
    }

    //Set the animation bool in the animator
    public void FlipEntity(int facingDirection)
    {
        facingDirectionIndicator.Rotate(0f, 180f * facingDirection, 0f);
    }

    //Set the animation bool in the animator
    public void SetAnimationBool(string animBoolName, bool value)
    {
        animator.SetBool(animBoolName, value);
    }

    //Set the animation float in the animator
    public void SetAnimationFloat(string animFloatName, float value)
    {
        animator.SetFloat(animFloatName, value);
    }

    //Do stuff in states on animation triggers
    protected virtual void AnimationTrigger() => stateMachine.AnimationTrigger();
    protected virtual void AnimationFinishedTrigger() => stateMachine.AnimationFinishedTrigger();
}
