using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Grab State", menuName = "States/Player/Touching Wall/Wall Grab State")]

public class PlayerWallGrabState : PlayerTouchingWallState
{
    [SerializeField] protected PlayerWallSlideState wallSlideState;
    [SerializeField] protected PlayerWallClimbState wallClimbState;
    
    public override void Enter()
    {
        base.Enter();

        _player.FreezeInPlace();
    }
    
    public override void Exit()
    {
        base.Exit();

        _player.LetGoOfWall();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_stats.IsMovingUp)
        {
            return wallClimbState;
        }
        else if (_stats.IsMovingDown || !_stats.IsPressingGrab)
        {
            return wallSlideState;
        }

        return null;
    }
}
