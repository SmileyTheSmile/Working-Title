using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiniteStateMachine : CoreComponent
{
    [SerializeField] private GenericState _startingState;
    [SerializeField] private GenericState _currentState;

    [SerializeField] private List<GenericState> _states = new List<GenericState>();

    //Update the current state's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        _currentState.DoActions();
        GenericState nextState = _currentState.DoTransitions();
        LogComponentInfo();

        if (nextState != null)
        {
            ChangeState(nextState);
        }
    }

    //Update the current state's physics (FixedUpdate)
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _currentState.PhysicsUpdate();
    }

    //Start the state machine
    public override void Initialize(Core core)
    {
        base.Initialize(core);

        foreach (var state in _states)
        {
            state.SetCore(core);
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

    //Write the current state's name in the console
    public override void LogComponentInfo()
    {
        Debug.Log(_currentState.ToString());
    }

    //Do stuff in states on animation triggers
    public void AnimationTrigger() => _currentState.AnimationTrigger();
    public void AnimationFinishedTrigger() => _currentState.AnimationFinishedTrigger();
}
