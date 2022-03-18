using UnityEngine;

public class PlayerCrouchJumpState : PlayerJumpState
{
    public PlayerCrouchJumpState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        //core.movement.SetColliderHeight(playerData.crouchColliderHeight, ColliderLocation.top);

        //core.collisionSenses.ceilingCheck.transform.position -= new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
        //core.collisionSenses.groundCheck.transform.position += new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
    }

    public override void Exit()
    {
        base.Exit();

        //core.movement.SetColliderHeight(playerData.standColliderHeight, ColliderLocation.top);

        //core.collisionSenses.ceilingCheck.transform.position += new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
        //core.collisionSenses.groundCheck.transform.position -= new Vector3(0f, (playerData.standColliderHeight - playerData.crouchColliderHeight) / 2, 0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
