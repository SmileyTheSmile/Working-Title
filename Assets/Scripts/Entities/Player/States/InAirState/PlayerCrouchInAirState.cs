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

        if (!crouchInput && !isGrounded)
        {
            stateMachine.ChangeState(player.inAirState);
        }
    }

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
}
