using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;

    public PlayerWallGrabState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        holdPosition = player.transform.position;

        HoldPosition();
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

    private void HoldPosition()
    {
        player.transform.position = holdPosition;

        movement?.SetVelocityZero();
    }
}
