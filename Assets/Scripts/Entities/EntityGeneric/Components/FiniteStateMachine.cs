using UnityEngine;

public class FiniteStateMachine : CoreComponent
{
    private GenericState currentState;

    //Update the current state's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentState.LogicUpdate();
    }

    //Update the current state's physics (FixedUpdate)
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        currentState.PhysicsUpdate();
    }

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

    //Write the current state's name in the console
    public override void LogComponentInfo()
    {
        Debug.Log(currentState.ToString());
    }

    //Do stuff in states on animation triggers
    public void AnimationTrigger() => currentState.AnimationTrigger();
    public void AnimationFinishedTrigger() => currentState.AnimationFinishedTrigger();
}
