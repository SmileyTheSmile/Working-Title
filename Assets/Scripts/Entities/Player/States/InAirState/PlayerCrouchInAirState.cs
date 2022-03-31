using UnityEngine;

public class PlayerCrouchInAirState : PlayerInAirState
{
    private int amountOfCrouchesLeft;

    public PlayerCrouchInAirState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfCrouchesLeft = playerData.amountOfCrouches;
    }

    public override void Enter()
    {
        base.Enter();

        crouchInput = player.inputHandler.crouchInput;

        DecreaseAmountOfCrouchesLeft();

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

        if (!crouchInput && !isGrounded)
        {
            stateMachine.ChangeState(player.inAirState);
        }
    }

    #region Utility Functions

    public bool CanCrouch()
    {
        if (amountOfCrouchesLeft > 0)
        {
            return true;
        }

        return false;
    }

    public void ResetAmountOfCrouchesLeft() => amountOfCrouchesLeft = playerData.amountOfCrouches;

    public void DecreaseAmountOfCrouchesLeft() => amountOfCrouchesLeft--;

    #endregion
}
