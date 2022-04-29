using UnityEngine;

public class FiniteStateMachine
{
    public GenericState currentState { get; private set; }

    //Start the state machine
    public void Initialize(GenericState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    //Change the current state of entity
    public void ChangeState(GenericState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //Update the current state's logic (FixedUpdate)
    public void LogicUpdate()
    {
        currentState.LogicUpdate();
    }

    //Update the current state's physics (Update)
    public void PhysicsUpdate()
    {
        currentState.PhysicsUpdate();
    }

    //Write the current state's name in the console
    public void LogCurrentState()
    {
        Debug.Log(currentState.ToString());
    }

    //Do stuff in states on animation triggers
    public void AnimationTrigger() => currentState.AnimationTrigger();
    public void AnimationFinishedTrigger() => currentState.AnimationFinishedTrigger();
}
