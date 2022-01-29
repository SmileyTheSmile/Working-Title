using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;

    protected float startTime;

    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();

        player.animator.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;

        //Debug.Log(animBoolName);
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {
        Debug.Log(player.facingDirection);
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishedTrigger()
    {
        isAnimationFinished = true;
    }
}
