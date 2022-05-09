using UnityEngine;

public class FiniteStateMachine : CoreComponent
{
    private GenericState _currentState;

    //Update the current state's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _currentState.LogicUpdate();
    }

    //Update the current state's physics (FixedUpdate)
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _currentState.PhysicsUpdate();
    }

    //Start the state machine
    public void Initialize(GenericState startingState)
    {
        _currentState = startingState;
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
