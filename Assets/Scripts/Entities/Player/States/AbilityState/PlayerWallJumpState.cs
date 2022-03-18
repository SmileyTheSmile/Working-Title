using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    #region Utility Variables

    public Vector2 wallJumpAngle;
    private int wallJumpDirection;

    #endregion

    #region State Functions

    public PlayerWallJumpState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        wallJumpAngle = (Vector2)(Quaternion.Euler(0, 0, playerData.wallJumpAngle) * Vector2.right); //Temporary

        player.inputHandler.UseJumpInput();

        player.jumpState.ResetAmountOfJumpsLeft();
        player.jumpState.DecreaseAmountOfJumpsLeft();

        core.movement.SetVelocity(playerData.wallJumpVelocity, wallJumpAngle, wallJumpDirection);
        core.movement.CheckIfShouldFlip(wallJumpDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.animator.SetFloat("velocityX", core.movement.currentVelocity.y);
        player.animator.SetFloat("velocityY", Mathf.Abs(core.movement.currentVelocity.x));

        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion

    #region Utility Functions

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -core.movement.facingDirection;
        }
        else
        {
            wallJumpDirection = core.movement.facingDirection;
        }
    }

    #endregion
}
