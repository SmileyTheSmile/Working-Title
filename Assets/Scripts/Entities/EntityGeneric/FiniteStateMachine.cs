using UnityEngine;

public class FiniteStateMachine
{
    public GenericState currentState { get; private set; }
    public GenericState lastState { get; private set; }

    public void Initialize(GenericState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(GenericState newState)
    {
        currentState.Exit();
        lastState = currentState;
        currentState = newState;
        currentState.Enter();
    }

    public void LogCurrentState()
    {
        Debug.Log(currentState.ToString());
    }
}
