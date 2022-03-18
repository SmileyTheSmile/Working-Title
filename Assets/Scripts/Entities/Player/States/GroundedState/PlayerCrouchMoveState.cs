using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    #region State Functions

    public PlayerCrouchMoveState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        core.movement.SquashColliderDown(playerData.crouchColliderHeight);

        core.collisionSenses.ceilingCheck.transform.position -= new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
    }

    public override void Exit()
    {
        base.Exit();

        core.movement.SquashColliderDown(playerData.standColliderHeight);

        core.collisionSenses.ceilingCheck.transform.position += new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.movement.SetVelocityX(playerData.crouchMovementVelocity * core.movement.facingDirection);
        core.movement.CheckIfShouldFlip(inputX);

        if (inputX == 0)
        {
            stateMachine.ChangeState(player.crouchIdleState);
        }
        else if (!crouchInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion
}
