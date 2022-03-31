using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    #region Utility Variables

    private int amountOfJumpsLeft;

    #endregion

    #region State Functions

    public PlayerJumpState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        player.inputHandler.UseJumpInput();
        core.movement.SetVelocityY(playerData.jumpVelocity);

        player.inAirState.SetIsJumping();
        player.crouchInAirState.SetIsJumping();

        DecreaseAmountOfJumpsLeft();
        isAbilityDone = true;
    }

    #endregion

    #region Utility Functions

    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            return true;
        }

        return false;
    }

    public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;

    #endregion
}