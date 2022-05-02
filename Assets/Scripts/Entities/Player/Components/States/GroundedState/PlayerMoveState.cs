using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movement?.SetVelocityX(playerData.movementVelocity * inputX);

        if (inputX == 0f)
        {
            if (crouchInput)
            {
                stateMachine?.ChangeState(player.crouchIdleState);
            }
            else
            {
                stateMachine?.ChangeState(player.idleState);
            }
        }
        else if (crouchInput)
        {
            stateMachine?.ChangeState(player.crouchMoveState);
        }
    }
}