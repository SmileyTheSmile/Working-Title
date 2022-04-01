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

        if (inputX != 0)
        {
            if (crouchInput || isTouchingCeiling)
            {
                stateMachine.ChangeState(player.crouchMoveState);
            }
            else
            {

                stateMachine.ChangeState(player.moveState);
            }
        }
        else if (!crouchInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    #endregion
}