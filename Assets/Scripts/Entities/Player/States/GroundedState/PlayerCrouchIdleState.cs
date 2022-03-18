using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    #region State Functions

    public PlayerCrouchIdleState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityZero();
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

        if (inputX != 0)
        {
            stateMachine.ChangeState(player.crouchMoveState);
        }
        else if (!crouchInput && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion
}
