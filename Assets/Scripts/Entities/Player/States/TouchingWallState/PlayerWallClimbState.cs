using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Climb State", menuName = "States/Player/Touching Wall/Wall Climb State")]

public class PlayerWallClimbState : PlayerTouchingWallState
{
    [SerializeField] protected PlayerWallGrabState wallGrabState;

    public override void Enter()
    {
        base.Enter();

        _player.Step();
    }

    public override void Exit()
    {
        base.Exit();

        _player.StopMovementSound();
    }

    public override void DoActions()
    {
        base.DoActions();

        _player.ClimbWall();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (!_stats.IsMovingUp)
        {
            return wallGrabState;
        }

        return null;
    }
}
