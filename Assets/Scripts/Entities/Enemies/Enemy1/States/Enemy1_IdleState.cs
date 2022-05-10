using UnityEngine;

public class Enemy1_IdleState : Enemy1State
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;

    protected float idleTime;

    public Enemy1_IdleState(Enemy1 enemy, EnemyData enemyData, string animBoolName)
    : base(enemy, enemyData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityX(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            //movement?.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= _startTime + idleTime)
        {
            isIdleTimeOver = true;
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(enemyData.minIdleTime, enemyData.maxIdleTime);
    }
}
