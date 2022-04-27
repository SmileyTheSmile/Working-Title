using UnityEngine;

public class Enemy1 : EntityGeneric
{
    public Enemy1_IdleState idleState { get; private set; }
    public Enemy1_MoveState moveState { get; private set; }

    [SerializeField] protected EnemyData enemyData;

    protected override void Awake()
    {
        base.Awake();

        moveState = new Enemy1_MoveState(this, stateMachine, enemyData, "move");
        idleState = new Enemy1_IdleState(this, stateMachine, enemyData, "idle");

    }

    protected override void Start()
    {
        stateMachine.Initialize(moveState);
    }
}
