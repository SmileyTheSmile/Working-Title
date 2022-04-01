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

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool oldIsTouchingWall;
    protected bool isTouchingWallBack;
    protected bool oldIsTouchingWallBack;
    protected bool isTouchingLedge;
    protected bool isTouchingCeiling;
    protected bool isJumping;

    #endregion

    #region Utility Variables

    private float wallJumpCoyoteTimeStart;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private float airControlPercentage = 1f;

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

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall && oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();

        StartCoyoteTime();
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

        inputX = player.inputHandler.normalizedInputX;
        inputY = player.inputHandler.normalizedInputY;
        dashInput = player.inputHandler.dashInput;
        grabInput = player.inputHandler.grabInput;
        crouchInput = player.inputHandler.crouchInput;
        jumpInput = player.inputHandler.jumpInput;
        jumpInputStop = player.inputHandler.jumpInputStop;

        CheckJumpMultiplier();
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        //Ability States
        if (player.inputHandler.attackInputs[(int)CombatInputs.primary])
        {
            //stateMachine.ChangeState(player.primaryAttackState);
        }
        else if (player.inputHandler.attackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.secondaryAttackState);
        }
        else if (dashInput && player.dashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
        else if (jumpInput && player.jumpState.CanJump())
        {
            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchJumpState);
            }
            else if (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime)
            {
                StopWallJumpCoyoteTime();
                isTouchingWall = core.collisionSenses.WallFront;
                player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.wallJumpState);
            }
            else
            {
                stateMachine.ChangeState(player.jumpState);
            }
        }

        if (isGrounded && core.movement.currentVelocity.y < 0.01)
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
        else if (isTouchingWall)
        {
            if (grabInput && isTouchingLedge)
            {
                stateMachine.ChangeState(player.wallGrabState);
            }
            else if (inputX == core.movement.facingDirection && core.movement.currentVelocity.y <= 0f)
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
            else if (!isTouchingLedge && !isGrounded && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.ledgeClimbState);
            }
        }
        else if (crouchInput && player.crouchInAirState.CanCrouch() && !isGrounded)
        {
            stateMachine.ChangeState(player.crouchInAirState);
        }
        else
        {
            core.movement.CheckIfShouldFlip(inputX);
            core.movement.SetVelocityX(playerData.movementVelocity * inputX * airControlPercentage);

            player.animator.SetFloat("velocityX", Mathf.Abs(core.movement.currentVelocity.x));
            player.animator.SetFloat("velocityY", core.movement.currentVelocity.y);
        }
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

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;

    #endregion
}