using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityZero();

        player.SetColliderHeight(playerData.crouchColliderHeight);
        core.collisionSenses.ceilingCheck.transform.position -= new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetColliderHeight(playerData.standColliderHeight);
        core.collisionSenses.ceilingCheck.transform.position += new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        if (inputX != 0)
        {
            stateMachine.ChangeState(player.crouchMoveState);
        }
        else if (inputY != -1 && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
