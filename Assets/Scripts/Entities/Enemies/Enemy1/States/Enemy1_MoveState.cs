using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_MoveState : Enemy1State
{
    #region Check Variables

    protected bool isDetectingWall;
    protected bool isDetectingLedge;

    #endregion

    #region State Functions

    public Enemy1_MoveState(Enemy1 enemy, FiniteStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    : base(enemy, stateMachine, enemyData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityX(enemyData.movementVelocity * core.movement.facingDirection);

        isDetectingLedge = core.collisionSenses.LedgeVertical;
        isDetectingWall = core.collisionSenses.WallFront;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDetectingWall || isDetectingLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        isDetectingLedge = core.collisionSenses.LedgeVertical;
        isDetectingWall = core.collisionSenses.WallFront;
    }

    #endregion
}
