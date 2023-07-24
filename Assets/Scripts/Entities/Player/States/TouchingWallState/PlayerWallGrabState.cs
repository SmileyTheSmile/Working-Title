using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Grab State", menuName = "States/Player/Touching Wall/Wall Grab State")]

public class PlayerWallGrabState : PlayerTouchingWallState
{
    [SerializeField] protected PlayerWallSlideState wallSlideState;
    [SerializeField] protected PlayerWallClimbState wallClimbState;
    
    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.FreezeInPlace();
    }
    
    public override void Exit()
    {
        base.Exit();

        _temporaryComponent.LetGoOfWall();
    }

    public override void DoActions()
    {
        base.DoActions();

        _temporaryComponent.HoldPosition();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_conditions.IsMovingUp)
        {
            return wallClimbState;
        }
        else if (_conditions.IsMovingDown || !_conditions.IsPressingGrab)
        {
            return wallSlideState;
        }

        return null;
    }
}
