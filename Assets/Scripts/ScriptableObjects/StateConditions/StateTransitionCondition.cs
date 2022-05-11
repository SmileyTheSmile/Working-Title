using UnityEngine;

public class StateTransitionCondition : ScriptableObject
{
    public virtual bool IsMet() => false;
}