using UnityEngine;

public class EnemyState : GenericState
{
    protected EnemyData enemyData;

    public EnemyState(EnemyData enemyData, string animBoolName)
    : base(animBoolName)
    {
        this.enemyData = enemyData;
    }
}