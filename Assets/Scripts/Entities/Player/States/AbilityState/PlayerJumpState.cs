using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    #region Utility Variables

    private int amountOfJumpsLeft;

    #endregion

    #region State Functions

    public PlayerJumpState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
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