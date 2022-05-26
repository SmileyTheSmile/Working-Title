using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiniteStateMachine : CoreComponent
{
    [SerializeField] private GenericState _startingState;
    [SerializeField] private List<GenericState> _states = new List<GenericState>();
    
    private GenericState _currentState;

    //Update the current state's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        _currentState.DoActions();
        GenericState nextState = _currentState.DoTransitions();

        if (nextState != null)
        {
            ChangeState(nextState);
        }
    }

    //Start the state machine
    public override void Initialize(EntityGeneric entity)
    {
        base.Initialize(entity); 

        //TODO Move core setup in a proper place and get rid on state list
        foreach (var state in _states)
        {
            state.SetCore(entity);
        }

        _currentState = _startingState;
        _currentState.Enter();
    }

    //Change the current state of entity
    public void ChangeState(GenericState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    //Do stuff in states on animation triggers
    public void AnimationTrigger() => _currentState.AnimationTrigger();
    public void AnimationFinishedTrigger() => _currentState.AnimationFinishedTrigger();
}
