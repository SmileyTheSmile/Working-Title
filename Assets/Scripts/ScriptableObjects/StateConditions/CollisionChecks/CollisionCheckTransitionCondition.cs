using UnityEngine;

[CreateAssetMenu(fileName = "New Collision Check Condition", menuName = "State Conditions/Collision Check")]

public class CollisionCheckTransitionCondition : StateTransitionCondition
{
    public bool value;

    public override bool IsMet(bool desiredResult) => value == desiredResult;
}
