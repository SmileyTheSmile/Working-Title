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

        player.SetVelocityZero();
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
            stateMachine.ChangeState(player.playerIdleState);
        }

        inputX = player.inputHandler.normalizedInputX;
        inputY = player.inputHandler.normalizedInputY;

        player.SetVelocityZero();
        player.transform.position = startPos;

        if (inputX == player.facingDirection && isHanging && !isClimbing)
        {
            isClimbing = true;
            player.animator.SetBool("ledgeClimb", true);
        }
        else if (inputY == -1 && isHanging && !isClimbing)
        {
            stateMachine.ChangeState(player.playerInAirState);
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

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
}
