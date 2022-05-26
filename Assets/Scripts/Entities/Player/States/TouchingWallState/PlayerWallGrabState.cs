using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Grab State", menuName = "States/Player/Touching Wall/Wall Grab State")]

public class PlayerWallGrabState : PlayerTouchingWallState
{
    protected PlayerWallSlideState wallSlideState => conditionManager.wallSlideState;
    protected PlayerWallClimbState wallClimbState => conditionManager.wallClimbState;

    private Vector2 _holdPosition;
    
    public override void Enter()
    {
        base.Enter();

        _holdPosition = _entity.transform.position;

        HoldPosition();
    }

    public override void DoActions()
    {
        base.DoActions();

        HoldPosition();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();

        if (parentResult != null)
        {
            return parentResult;
        }

        if (_isMovingUp)
        {
            return wallClimbState;
        }
        else if (_isMovingDown || !_isPressingGrab)
        {
            return wallSlideState;
        }

        return null;
    }

    private void HoldPosition()
    {
        _entity.transform.position = _holdPosition;

        movement.SetVelocityZero();
    }
}