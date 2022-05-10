using UnityEngine;

[CreateAssetMenu(fileName = "Player Ledge Climb State", menuName = "States/Player/Ability/Ledge Climb State")]

public class PlayerLedgeClimbState : PlayerAbilityState
{
    public PlayerLedgeClimbState(Player player,  PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityY(_playerData.ledgeClimbVelocity);
        _isAbilityDone = true;
    }
}
