using UnityEngine;

public abstract class StateTransitionCondition : ScriptableObject
{
    public abstract bool IsMet(bool desiredResult);
}