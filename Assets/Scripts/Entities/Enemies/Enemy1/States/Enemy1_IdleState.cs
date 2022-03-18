using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_IdleState : Enemy1State
{
    #region Utility Variables

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;

    protected float idleTime;

    #endregion

    #region State Functions

    public Enemy1_IdleState(Enemy1 enemy, FiniteStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    : base(enemy, stateMachine, enemyData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityX(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            core.movement.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion

    #region Utility Functions

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(enemyData.minIdleTime, enemyData.maxIdleTime);
    }

    #endregion
}
