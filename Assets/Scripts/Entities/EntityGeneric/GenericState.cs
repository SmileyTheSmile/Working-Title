using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericState : ScriptableObject
{
    [SerializeField] private List<StateTransition> _transitions = new List<StateTransition>();

    [SerializeField] protected string _animBoolName;

    protected Core _core;
    protected float _startTime;
    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    //What to do when entering the state
    public virtual void Enter() 
    {
        _startTime = Time.time;
        _isExitingState = false;
        _isAnimationFinished = false;
    }

    //What to do when exiting the state
    public virtual void Exit()
    {
        _isExitingState = true;
    }
    
    public virtual void SetCore(Core core)
    {
        _core = core;
    }
    
    //Update the component's physics (FixedUpdate)
    public virtual void PhysicsUpdate() { }
    //Do all the checks if the state should transition into another state
    public virtual void DoActions() { if (_isExitingState) return; }
    //Execute all the actions the state must do every Update
    public abstract GenericState DoTransitions();
    //What to do in animation events in Animator
    public virtual void AnimationTrigger() { }
    //What to do on finished animation in Animator
    public virtual void AnimationFinishedTrigger() => _isAnimationFinished = true; 
}
