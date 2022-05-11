using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Condition", menuName = "State Conditions/Generic")]

public class StateTransitionCondition : ScriptableObject
{
    public bool value;
    public bool IsMet() => value;
}