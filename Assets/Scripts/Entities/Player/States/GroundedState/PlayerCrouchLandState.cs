using UnityEngine;

public class PlayerCrouchLandState : PlayerGroundedState
{
    public PlayerCrouchLandState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0)
        {
            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchMoveState);
            }
            else
            {
                core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                stateMachine.ChangeState(player.moveState);
            }
        }
        else
        {
            if (isAnimationFinished)
            {
                if (crouchInput)
                {
                    stateMachine.ChangeState(player.crouchIdleState);
                }
                else
                {
                    core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                    stateMachine.ChangeState(player.idleState);
                }
            }
        }
    }
}
