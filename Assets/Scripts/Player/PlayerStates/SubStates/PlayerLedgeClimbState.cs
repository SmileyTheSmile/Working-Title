using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;

    private bool isHanging;
    private bool isClimbing;
    private bool isTouchingCeiling;
    private bool jumpInput;

    private int inputX;
    private int inputY;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityY(playerData.jumpVelocity);
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();

        startPos.Set(cornerPos.x - (player.facingDirection * playerData.startOffset.x), cornerPos.y);
        stopPos.Set(cornerPos.x + (player.facingDirection * playerData.startOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }

            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            inputX = player.inputHandler.normalizedInputX;
            inputY = player.inputHandler.normalizedInputY;
            jumpInput = player.inputHandler.jumpInput;

            core.movement.SetVelocityZero();
            player.transform.position = startPos;

            if (inputX == player.facingDirection && isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;
                player.animator.SetBool("ledgeClimb", true);
            }
            else if (inputY == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.inAirState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.wallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.wallJumpState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        player.animator.SetBool("ledgeClimb", false);
    }

    public void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * player.facingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, playerData.whatIsGround);
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
}
