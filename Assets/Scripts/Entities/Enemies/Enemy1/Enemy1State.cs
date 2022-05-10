using UnityEngine;

public class Enemy1State : EnemyState
{
    protected Enemy1 enemy;

    public Enemy1State(Enemy1 enemy, EnemyData enemyData, string animBoolName)
    : base(enemyData, animBoolName)
    {
        this.enemy = enemy;

        core = enemy.core;
    }

    public override void Enter() //What to do when entering the state
    {
        base.Enter();

        visualController.SetAnimationBool(_animBoolName, true);
    }

    public override void Exit() //What to do when exiting the state
    {
        base.Exit();

        visualController.SetAnimationBool(_animBoolName, false);
    }
}
