using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    #region Utility Variables

    private Vector2 holdPosition;

    #endregion

    #region State Functions

    public PlayerWallGrabState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        holdPosition = player.transform.position;

        HoldPosition();
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

        HoldPosition();

        if (inputY > 0)
        {
            stateMachine.ChangeState(player.wallClimbState);
        }
        else if (inputY < 0 || !grabInput)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion

    #region Wall Grab Functions

    private void HoldPosition()
    {
        player.transform.position = holdPosition;

        core.movement.SetVelocityZero();
    }

    #endregion
}
