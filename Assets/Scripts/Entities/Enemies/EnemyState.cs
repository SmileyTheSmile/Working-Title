public class EnemyState : GenericState
{
    protected EnemyData enemyData;

    public EnemyState(FiniteStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    : base(stateMachine, animBoolName)
    {
        this.enemyData = enemyData;
    }
}