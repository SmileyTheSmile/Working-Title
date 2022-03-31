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

        DecreaseAmountOfCrouchesLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!crouchInput && !isGrounded)
        {
            core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

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
