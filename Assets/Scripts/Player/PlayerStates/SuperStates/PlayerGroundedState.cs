using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState {

    protected Vector2 input;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolNime) : base(player, stateMachine, playerData, animBoolNime) {}

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        input = player.inputHandler.movementInput;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
