using UnityEngine;

[CreateAssetMenu(fileName = "Player Ledge Climb State", menuName = "States/Player/Ability/Ledge Climb State")]

public class PlayerLedgeClimbState : PlayerAbilityState
{
    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.ClimbLedge();
    }
}
