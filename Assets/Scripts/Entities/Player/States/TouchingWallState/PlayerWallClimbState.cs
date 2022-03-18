using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    #region State Functions
    public PlayerWallClimbState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

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

        if (isExitingState)
        {
            return;
        }

        core.movement.SetVelocityY(playerData.wallClimbVelocity);

        if (inputY != 1)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion
}
