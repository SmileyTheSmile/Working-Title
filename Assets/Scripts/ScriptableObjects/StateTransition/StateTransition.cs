using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Transition", menuName = "States/Transition")]

[Serializable]
public class StateTransition
{
    [SerializeField] private GenericState nextState = null;
    [SerializeField] private string expression;
    [SerializeField] private List<ConditionUsage> conditions = new List<ConditionUsage>();

    public GenericState NextState => nextState;

    public bool ShouldTransition()
    {
        foreach (var condition in conditions)
        {
            if (!condition.Condition.IsMet(condition.ExpectedResult))
            {
                return false;
            }
        }

        return true;
    }

    [Serializable]
    public struct ConditionUsage
    {
        public bool ExpectedResult;
        public int id;
        public StateTransitionCondition Condition;
    }
}

