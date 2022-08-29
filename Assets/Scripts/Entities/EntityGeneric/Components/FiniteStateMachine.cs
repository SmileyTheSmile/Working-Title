using System.Collections.Generic;
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
    public override void Initialize(EntityCore entity)
    {
        base.Initialize(entity); 

        //TODO Move core setup in a proper place and get rid of state list
        foreach (var state in _states)
        {
            state.Initialize(entity);
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
}
