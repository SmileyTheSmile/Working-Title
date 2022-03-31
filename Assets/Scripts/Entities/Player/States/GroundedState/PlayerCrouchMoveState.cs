using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    #region State Functions

    public PlayerCrouchMoveState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        crouchInput = player.inputHandler.crouchInput;

        if (core.movement.crouchingForm == PlayerCrouchingForm.normal && crouchInput)
        {
            core.SquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

            core.movement.crouchingForm = PlayerCrouchingForm.crouchingDown;
        }
    }

    public override void Exit()
    {
        base.Exit();

        crouchInput = player.inputHandler.crouchInput;

        if ((!crouchInput && core.movement.crouchingForm == PlayerCrouchingForm.crouchingDown && !isTouchingCeiling) || (!isTouchingWall && !isTouchingCeiling))
        {
            core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

            core.movement.crouchingForm = PlayerCrouchingForm.normal;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.movement.SetVelocityX(playerData.crouchMovementVelocity * core.movement.facingDirection);
        core.movement.CheckIfShouldFlip(inputX);

        if (inputX != 0f)
        {
            if (!crouchInput && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
        else
        {
            if (!crouchInput && !isTouchingCeiling)
            {
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
