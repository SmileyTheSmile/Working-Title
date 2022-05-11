using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericState : ScriptableObject
{
    protected VisualController visualController
    { get => _visualController ?? core.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    protected FiniteStateMachine stateMachine
    { get => _stateMachine ?? core.GetCoreComponent(ref _stateMachine); }
    private FiniteStateMachine _stateMachine;

    [SerializeField] private List<StateTransition> transitions = new List<StateTransition>();

    protected Core core;

    protected float _startTime;
    protected bool _isAnimationFinished;
    protected bool _isExitingState;
    protected string _animBoolName;

    public GenericState(string animBoolName)
    {
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

        DoActions();
        DoTransitions();
    }

    //Update the component's physics (FixedUpdate)
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public void ProcessTransitions()
    {
        // Loop over all of the possible transitions from this state
        foreach (var transition in transitions)
        {
            // Check to see if the particular transition conditions are met
            if (transition.ShouldTransition())
            {
                // Let the caller know which state we should transition to
                stateMachine.ChangeState(transition.NextState);
            }
        }
    }

    //Do all the checks if the state should transition into another state
    public virtual void DoTransitions() { }
    //Execute all the actions the state must do every Update
    public virtual void DoActions() { }
    //Update all check variables
    public virtual void DoChecks() { }
    //What to do in animation events in Animator
    public virtual void AnimationTrigger() { }
    //What to do on finished animation in Animator
    public virtual void AnimationFinishedTrigger() => _isAnimationFinished = true; 
}
