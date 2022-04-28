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

        movement?.CrouchDown(playerData.standColliderHeight, playerData.crouchColliderHeight, crouchInput);
    }

    public override void Exit()
    {
        base.Exit();

        crouchInput = player.inputHandler.crouchInput;

        movement?.UnCrouchDown(playerData.standColliderHeight, playerData.crouchColliderHeight, crouchInput);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movement?.SetVelocityX(playerData.crouchMovementVelocity * movement.facingDirection);

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
