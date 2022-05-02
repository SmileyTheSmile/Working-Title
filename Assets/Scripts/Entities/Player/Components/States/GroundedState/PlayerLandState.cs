using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0)
        {
            if (crouchInput)
            {
                stateMachine?.ChangeState(player.crouchMoveState);
            }
            else
            {
                stateMachine?.ChangeState(player.moveState);
            }
        }
        else
        {
            if (isAnimationFinished)
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
        }
    }
}
