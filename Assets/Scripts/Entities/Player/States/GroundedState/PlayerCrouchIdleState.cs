using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    #region State Functions

    public PlayerCrouchIdleState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0)
        {
            if (crouchInput || isTouchingCeiling)
            {
                stateMachine.ChangeState(player.crouchMoveState);
            }
            else
            {
                core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                stateMachine.ChangeState(player.moveState);
            }
        }
        else if (!crouchInput && !isTouchingCeiling)
        {
            core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

            stateMachine.ChangeState(player.idleState);
        }
    }

    #endregion
}
