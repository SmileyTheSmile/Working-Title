using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Attack State", menuName = "States/Player/Ability/Attack State")]

public class PlayerAttackState : PlayerAbilityState
{
    public override void Enter()
    {
        _isAbilityDone = true;
    }
}
