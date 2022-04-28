using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    protected int inputX;
    protected int inputY;
    protected bool dashInput;
    protected bool grabInput;
    protected bool crouchInput;
    protected bool jumpInput;
    protected bool jumpInputStop;
    protected Vector2 mousePositionInput;

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool oldIsTouchingWall;
    protected bool isTouchingWallBack;
    protected bool oldIsTouchingWallBack;
    protected bool isTouchingLedge;
    protected bool isTouchingCeiling;
    protected bool isJumping;

    private float wallJumpCoyoteTimeStart;
    private float airControlPercentage = 1f;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;

    public PlayerInAirState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData)
    {
        airControlPercentage = playerData.defaultAirControlPercentage;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = collisionSenses.Ground;
        isTouchingWall = collisionSenses.WallFront;
        isTouchingWallBack = collisionSenses.WallBack;
        isTouchingLedge = collisionSenses.LedgeHorizontal;

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
        mousePositionInput = player.inputHandler.mousePositionInput;

        CheckJumpMultiplier();
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        movement?.CheckIfShouldFlip(inputX, mousePositionInput.x);

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
                isTouchingWall = collisionSenses.WallFront;
                player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.wallJumpState);
            }
            else
            {
                stateMachine.ChangeState(player.jumpState);
            }
        }

        if (isGrounded && movement.currentVelocity.y < 0.01)
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
        else if (isTouchingWall && !crouchInput)
        {
            if (grabInput && isTouchingLedge)
            {
                stateMachine.ChangeState(player.wallGrabState);
            }
            else if (inputX == movement.facingDirection && movement.currentVelocity.y <= 0f)
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
            movement?.SetVelocityX(playerData.movementVelocity * inputX * airControlPercentage);

            player.animator.SetFloat("velocityX", Mathf.Abs(movement.currentVelocity.x));
            player.animator.SetFloat("velocityY", movement.currentVelocity.y);
        }
    }

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
            movement?.SetVelocityY(movement.currentVelocity.y * playerData.variableJumpHeightMultiplier);
            isJumping = false;
        }
        else if (movement.currentVelocity.y <= 0f)
        {
            isJumping = false;
        }
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;
}