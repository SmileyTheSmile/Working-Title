using UnityEngine;

[CreateAssetMenu(fileName = "New Collision Check", menuName = "State Conditions/Collision Check")]

public class StateCollisionCheckCondition : StateTransitionCondition
{
    public bool value;

    public override bool IsMet() => value;
}
