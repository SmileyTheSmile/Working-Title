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
    private bool isTouchingCeiling;
    private bool dashInput;

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

        jumpInput = player.inputHandler.jumpInput;
        grabInput = player.inputHandler.grabInput;
        dashInput = player.inputHandler.dashInput;
        inputX = player.inputHandler.normalizedInputX;
        jumpInputStop = player.inputHandler.jumpInputStop;

        CheckJumpMultiplier();

        if (player.inputHandler.attackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        else if (player.inputHandler.attackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.secondaryAttackState);
        }
        else if (isGrounded && player.currentVelocity.y < 0.01)
        {
            stateMachine.ChangeState(player.landState);
        }
        //else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        //{
        //    stateMachine.ChangeState(player.ledgeClimbState);
        //}
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = player.CheckIfTouchingWall();
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
        else if (isTouchingWall && inputX == player.facingDirection && player.currentVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        else if (dashInput && player.dashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
        else
        {
            player.CheckIfShouldFlip(inputX);
            core.movement.SetVelocityX(playerData.movementVelocity * inputX);

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
            core.movement.SetVelocityY(player.currentVelocity.y * playerData.variableJumpHeightMultiplier);
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
            player.jumpState.DecreaseAmountOfJumpsLeft();
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
