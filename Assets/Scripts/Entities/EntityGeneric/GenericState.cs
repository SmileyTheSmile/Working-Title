using System.Collections.Generic;
using UnityEngine;

public abstract class GenericState : ScriptableObject
{
    [SerializeField] protected List<StateTransition> _transitions = new List<StateTransition>();
    [SerializeField] protected string _animBoolName;

    protected Core _core;
    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    public virtual void Enter() 
    {
        _isExitingState = false;
        _isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        _isExitingState = true;
    }
    
    public virtual void Initialize(Core entity)
    {
        _core = entity;
    }
    
    //Do all the checks if the state should transition into another state
    public virtual void DoActions() { if (_isExitingState) return; }
    //Execute all the actions the state must do every Update
    public abstract GenericState DoTransitions(); //TODO Create a universal transition system with binary trees
    //What to do in animation events in Animator
    public virtual void AnimationTrigger() { }
    //What to do on finished animation in Animator
    public virtual void AnimationFinishedTrigger() => _isAnimationFinished = true; 
}
