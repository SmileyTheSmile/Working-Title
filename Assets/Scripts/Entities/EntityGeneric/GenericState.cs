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

    public float startTime;
    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected string animBoolName;

    public GenericState(FiniteStateMachine stateMachine, string animBoolName)
    {
        this._stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() //What to do when entering the state
    {
        DoChecks();

        startTime = Time.time;
        isExitingState = false;
        isAnimationFinished = false;
    }

    public virtual void Exit() //What to do when exiting the state
    {
        isExitingState = true;
    }

    public virtual void LogicUpdate() //What to do in the Update() function
    {
        if (isExitingState)
        {
            return;
        }
    }

    public virtual void PhysicsUpdate() //What to do in the FixedUpdate() function
    {
        DoChecks();
    }

    public virtual void DoChecks() { } //Update all check variables

    public virtual void AnimationTrigger() { } //What to do in animation events in Animator

    public virtual void AnimationFinishedTrigger() => isAnimationFinished = true; //What to do on finished animation in Animator
}
