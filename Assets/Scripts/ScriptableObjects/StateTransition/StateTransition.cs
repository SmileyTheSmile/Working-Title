using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transition", menuName = "States/Transition")]

[Serializable]
public class StateTransition : ScriptableObject
{
    [SerializeField] private GenericState nextState = null;
    [SerializeField] private List<StateTransitionCondition> conditions = new List<StateTransitionCondition>();

    public GenericState NextState => nextState;

    public bool ShouldTransition()
    {
        foreach (var condition in conditions)
        {
            if (!condition.IsMet())
            {
                return false;
            }
        }

        return true;
    }
}