using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Slide State", menuName = "States/Player/Touching Wall/Wall Slide State")]

public class PlayerWallSlideState : PlayerTouchingWallState
{
    protected PlayerWallGrabState wallGrabState => conditionManager.wallGrabState;
    
    public override void DoActions()
    {
        base.DoActions();

        movement?.SetVelocityY(-_playerData.wallSlideVelocity);
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();

        if (parentResult != null)
        {
            return parentResult;
        }

        if (_isPressingGrab && !_isMovingUp && !_isMovingDown)
        {
            return wallGrabState;
        }

        return null;
    }
}