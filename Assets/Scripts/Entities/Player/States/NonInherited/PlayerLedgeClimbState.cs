using UnityEngine;

public class PlayerLedgeClimbState : PlayerAbilityState
{
    public PlayerLedgeClimbState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityY(playerData.ledgeClimbVelocity);
        isAbilityDone = true;
    }
}
