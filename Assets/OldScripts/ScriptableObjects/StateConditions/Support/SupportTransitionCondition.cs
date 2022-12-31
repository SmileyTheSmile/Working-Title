using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Support Condition", menuName = "State Conditions/Support")]

public class SupportTransitionCondition : StateTransitionCondition
{
    public bool value;

    public override bool IsMet(bool desiredResult) => value == desiredResult;
}
