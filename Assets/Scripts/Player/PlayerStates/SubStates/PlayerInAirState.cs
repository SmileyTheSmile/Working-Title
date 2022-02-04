using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{

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

    private int inputX;

    private float startWallCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();

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
        Debug.Log(isTouchingWall);

        CheckJumpMultiplier();

        if (isGrounded && player.currentVelocity.y < 0.01)
        {
            stateMachine.ChangeState(player.playerLandState);
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
            player.SetVelocityX(playerData.movementVelocity * inputX);

            player.animator.SetFloat("velocityX", player.currentVelocity.x);
            player.animator.SetFloat("velocityY", player.currentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
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
