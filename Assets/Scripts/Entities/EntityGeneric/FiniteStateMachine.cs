using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public GenericState currentState { get; private set; }

    public void Initialize(GenericState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(GenericState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
