using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericState
{
    protected VisualController visualController
    { get => _visualController ?? core.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    protected FiniteStateMachine stateMachine
    { get => _stateMachine ?? core.GetCoreComponent(ref _stateMachine); }
    private FiniteStateMachine _stateMachine;

    protected Core core;

    protected float startTime;
    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected string animBoolName;

    public GenericState(FiniteStateMachine stateMachine, string animBoolName)
    {
        this._stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    //What to do when entering the state
    public virtual void Enter() 
    {
        DoChecks();

        startTime = Time.time;
        isExitingState = false;
        isAnimationFinished = false;
    }

    //What to do when exiting the state
    public virtual void Exit()
    {
        isExitingState = true;
    }

    //Update the component's logic (Update)
    public virtual void LogicUpdate()
    {
        if (isExitingState)
        {
            return;
        }
    }

    //Update the component's physics (FixedUpdate)
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    //Update all check variables
    public virtual void DoChecks() { }
    //What to do in animation events in Animator
    public virtual void AnimationTrigger() { }
    //What to do on finished animation in Animator
    public virtual void AnimationFinishedTrigger() => isAnimationFinished = true; 
}
