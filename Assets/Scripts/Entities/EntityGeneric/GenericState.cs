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

    protected float _startTime;
    protected bool _isAnimationFinished;
    protected bool _isExitingState;
    protected string _animBoolName;

    public GenericState(FiniteStateMachine stateMachine, string animBoolName)
    {
        this._stateMachine = stateMachine;
        this._animBoolName = animBoolName;
    }

    //What to do when entering the state
    public virtual void Enter() 
    {
        DoChecks();

        _startTime = Time.time;
        _isExitingState = false;
        _isAnimationFinished = false;
    }

    //What to do when exiting the state
    public virtual void Exit()
    {
        _isExitingState = true;
    }

    //Update the component's logic (Update)
    public virtual void LogicUpdate()
    {
        if (_isExitingState)
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
    public virtual void AnimationFinishedTrigger() => _isAnimationFinished = true; 
}
