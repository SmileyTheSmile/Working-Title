using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_MoveState : Enemy1State
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;

    public Enemy1_MoveState(Enemy1 enemy, FiniteStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    : base(enemy, stateMachine, enemyData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityX(enemyData.movementVelocity * movement.movementDirection);

        isDetectingLedge = collisionSenses.LedgeVertical;
        isDetectingWall = collisionSenses.WallFront;
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

        isDetectingLedge = collisionSenses.LedgeVertical;
        isDetectingWall = collisionSenses.WallFront;
    }
}
