using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    #region State Functions

    public PlayerLandState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0)
        {
            if (crouchInput)
            {
                core.SquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                stateMachine.ChangeState(player.crouchMoveState);
            }
            else
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
        else
        {
            if (isAnimationFinished)
            {
                if (crouchInput)
                {
                    core.SquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                    stateMachine.ChangeState(player.crouchIdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.idleState);
                }
            }
        }
    }

    #endregion
}
