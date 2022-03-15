using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;

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
        cornerPos = DetermineCornerPosition();

        startPos.Set(cornerPos.x - (core.movement.facingDirection * playerData.startOffset.x), cornerPos.y);
        stopPos.Set(cornerPos.x + (core.movement.facingDirection * playerData.startOffset.x), cornerPos.y + playerData.stopOffset.y);

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

            if (inputX == core.movement.facingDirection && isHanging && !isClimbing)
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
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * core.movement.facingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, core.collisionSenses.whatIsGround);
    }

    private Vector2 DetermineCornerPosition()
    {
        RaycastHit2D hitX = Physics2D.Raycast(core.collisionSenses.wallCheck.position, Vector2.right * core.movement.facingDirection, core.collisionSenses.wallCheckDistance, core.collisionSenses.whatIsGround);
        float distX = hitX.distance;
        workspace.Set(distX * core.movement.facingDirection, 0f);
        RaycastHit2D hitY = Physics2D.Raycast(core.collisionSenses.ledgeCheck.position + (Vector3)(workspace), Vector2.down, core.collisionSenses.ledgeCheck.position.y - core.collisionSenses.wallCheck.position.y + 0.15f, core.collisionSenses.whatIsGround);
        float distY = hitY.distance;
        workspace.Set(core.collisionSenses.wallCheck.position.x + (distX * core.movement.facingDirection), core.collisionSenses.ledgeCheck.position.y - distY);
        return workspace;
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
}
