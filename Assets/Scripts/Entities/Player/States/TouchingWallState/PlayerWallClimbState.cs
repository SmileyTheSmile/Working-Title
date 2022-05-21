using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Climb State", menuName = "States/Player/Touching Wall/Wall Climb State")]

public class PlayerWallClimbState : PlayerTouchingWallState
{
    protected PlayerWallGrabState wallGrabState => conditionManager.wallGrabState;
    
    public override void DoActions()
    {
        base.DoActions();

        movement.SetVelocityY(_playerData.wallClimbVelocity);
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();

        if (parentResult != null)
        {
            return parentResult;
        }

        if (!_isMovingUp)
        {
            return wallGrabState;
        }

        return null;
    }
}
