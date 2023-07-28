using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Slide State", menuName = "States/Player/Touching Wall/Wall Slide State")]

public class PlayerWallSlideState : PlayerTouchingWallState
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

        _player.SlideDown();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_stats.IsPressingGrab && !_stats.IsMovingUp && !_stats.IsMovingDown)
        {
            return wallGrabState;
        }

        return null;
    }
}