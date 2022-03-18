using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1State : EnemyState
{
    protected Enemy1 enemy;

    public Enemy1State(Enemy1 enemy, FiniteStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    : base(stateMachine, enemyData, animBoolName)
    {
        this.enemy = enemy;

        core = enemy.core;
    }

    public override void Enter() //What to do when entering the state
    {
        base.Enter();

        enemy.animator.SetBool(animBoolName, true);
    }

    public override void Exit() //What to do when exiting the state
    {
        base.Exit();

        enemy.animator.SetBool(animBoolName, false);
    }
}
