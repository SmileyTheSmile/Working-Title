using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

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

        core.movement.SetVelocityX(playerData.crouchMovementVelocity * core.movement.facingDirection);

        core.movement.CheckIfShouldFlip(inputX);

        if (inputX == 0)
        {
            stateMachine.ChangeState(player.crouchIdleState);
        }
        else if (inputY != -1 && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
