using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transition", menuName = "States/Transition")]

[Serializable]
public class StateTransition : ScriptableObject
{
    [SerializeField] private GenericState nextState = null;
    [SerializeField] private List<ConditionUsage> conditions = new List<ConditionUsage>();

    public GenericState NextState => nextState;

    public bool ShouldTransition()
    {
        bool result;
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
        public StateTransitionCondition Condition;
        public Operator Operator;
    }

    public enum Operator { And, Or }
}