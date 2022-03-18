using UnityEngine;

public class PlayerInAirState : PlayerState
{
    #region Input Variables

    protected int inputX;
    protected int inputY;
    protected bool dashInput;
    protected bool grabInput;
    protected bool crouchInput;
    protected bool jumpInput;
    protected bool jumpInputStop;

    #endregion

    #region Check Variables

    private bool isGrounded;
    private bool isTouchingWall;
    private bool oldIsTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;
    private bool isTouchingCeiling;
    private bool isJumping;

    #endregion

    #region Utility Variables

    private float wallJumpCoyoteTimeStart;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;

    #endregion

    #region State Functions

    public PlayerInAirState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = core.collisionSenses.Ground;
        isTouchingWall = core.collisionSenses.WallFront;
        isTouchingWallBack = core.collisionSenses.WallBack;
        isTouchingLedge = core.collisionSenses.LedgeHorizontal;

        //if (!isTouchingWall && !isTouchingLedge)
        //{
        //    player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        //}

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall && oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        inputX = player.inputHandler.normalizedInputX;
        inputY = player.inputHandler.normalizedInputY;
        dashInput = player.inputHandler.dashInput;
        grabInput = player.inputHandler.grabInput;
        crouchInput = player.inputHandler.crouchInput;
        jumpInput = player.inputHandler.jumpInput;
        jumpInputStop = player.inputHandler.jumpInputStop;

        CheckJumpMultiplier();

        if (player.inputHandler.attackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        else if (player.inputHandler.attackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.secondaryAttackState);
        }
        else if (isGrounded && core.movement.currentVelocity.y < 0.01)
        {
            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchLandState);
            }
            else
            {
                stateMachine.ChangeState(player.landState);
            }
        }
        //else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        //{
        //    stateMachine.ChangeState(player.ledgeClimbState);
        //}
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = core.collisionSenses.WallFront;
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.wallJumpState);
        }
        else if (jumpInput && player.jumpState.CanJump())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        else if (isTouchingWall && inputX == core.movement.facingDirection && core.movement.currentVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        else if (dashInput && player.dashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
        else
        {
            core.movement.CheckIfShouldFlip(inputX);
            core.movement.SetVelocityX(playerData.movementVelocity * inputX);

            player.animator.SetFloat("velocityX", Mathf.Abs(core.movement.currentVelocity.x));
            player.animator.SetFloat("velocityY", core.movement.currentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion

    #region Utility Functions
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.jumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StopCoyoteTime() => coyoteTime = false;

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > wallJumpCoyoteTimeStart + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
            wallJumpCoyoteTimeStart = Time.time;
        }
    }

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        wallJumpCoyoteTimeStart = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    private void CheckJumpMultiplier()
    {
        if (!isJumping)
        {
            return;
        }

        if (jumpInputStop)
        {
            core.movement.SetVelocityY(core.movement.currentVelocity.y * playerData.variableJumpHeightMultiplier);
            isJumping = false;
        }
        else if (core.movement.currentVelocity.y <= 0f)
        {
            isJumping = false;
        }
    }
    public void SetIsJumping() => isJumping = true;

    #endregion
}