using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{

    private bool isGrounded;
    private bool jumpInput;
    private bool grabInput;
    private bool coyoteTime;
    private bool isJumping;
    private bool jumpInputStop;
    private bool isTouchingWall;

    private int inputX;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
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

    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;
}
