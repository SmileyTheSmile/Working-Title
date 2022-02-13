using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int inputX;

    private float startWallCoyoteTime;

    private bool isGrounded;
    private bool jumpInput;
    private bool grabInput;
    private bool coyoteTime;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isJumping;
    private bool jumpInputStop;
    private bool wallJumpCoyoteTime;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isTouchingLedge;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
        isTouchingLedge = player.CheckIfTouchingLedge();

        if (!isTouchingWall && !isTouchingLedge)
        {
            player.playerLedgeClimbState.SetDetectedPosition(player.transform.position);
        }

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
        jumpInput = player.inputHandler.jumpInput;
        grabInput = player.inputHandler.grabInput;
        jumpInputStop = player.inputHandler.jumpInputStop;

        CheckJumpMultiplier();

        if (isGrounded && player.currentVelocity.y < 0.01)
        {
            stateMachine.ChangeState(player.playerLandState);
        }
        else if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.playerLedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = player.CheckIfTouchingWall();
            player.playerWallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.playerWallJumpState);
        }
        else if (jumpInput && player.playerJumpState.CanJump())
        {
            stateMachine.ChangeState(player.playerJumpState);
        }
        else if (isTouchingWall && grabInput)
        {
            stateMachine.ChangeState(player.playerWallGrabState);
        }
        else if (isTouchingWall && inputX == player.facingDirection && player.currentVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.playerWallSlideState);
        }
        else
        {
            player.CheckIfShouldFlip(inputX);
            player.SetVelocityX(playerData.movementVelocity * inputX);

            player.animator.SetFloat("velocityX", Mathf.Abs(player.currentVelocity.x));
            player.animator.SetFloat("velocityY", player.currentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckJumpMultiplier()
    {
        if (!isJumping)
        {
            return;
        }

        if (jumpInputStop)
        {
            player.SetVelocityY(player.currentVelocity.y * playerData.variableJumpHeightMultiplier);
            isJumping = false;
        }
        else if (player.currentVelocity.y <= 0f)
        {
            isJumping = false;
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.playerJumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
            startWallCoyoteTime = Time.time;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime() => wallJumpCoyoteTime = true;
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void SetIsJumping() => isJumping = true;
}
