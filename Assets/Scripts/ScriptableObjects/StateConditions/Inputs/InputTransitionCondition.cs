using UnityEngine;

[CreateAssetMenu(fileName = "New Input Condition", menuName = "State Conditions/Input")]

public class InputTransitionCondition : StateTransitionCondition
{
    public bool value;

    public override bool IsMet(bool desiredResult) => value == desiredResult;
}
