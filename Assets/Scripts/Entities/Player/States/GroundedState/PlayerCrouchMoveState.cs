using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    #region State Functions

    public PlayerCrouchMoveState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.movement.SetVelocityX(playerData.crouchMovementVelocity * core.movement.facingDirection);
        core.movement.CheckIfShouldFlip(inputX);

        if (inputX != 0f)
        {
            if (!crouchInput && !isTouchingCeiling)
            {
                core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                stateMachine.ChangeState(player.moveState);
            }
        }
        else
        {
            if (!crouchInput && !isTouchingCeiling)
            {
                core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
        }
    }

    #endregion
}
