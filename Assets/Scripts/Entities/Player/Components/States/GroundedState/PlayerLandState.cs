using UnityEngine;

[CreateAssetMenu(fileName = "Player Land State", menuName = "States/Player/Grounded/Land State")]

public class PlayerLandState : PlayerGroundedState
{
    protected PlayerCrouchMoveState crouchMoveState => conditionManager.crouchMoveState;
    protected PlayerMoveState moveState => conditionManager.moveState;
    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    protected PlayerIdleState idleState => conditionManager.idleState;
    
    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();

        if (parentResult != null)
        {
            return parentResult;
        }

        if (_isMovingX)
        {
            if (_isPressingCrouch)
            {
                return crouchMoveState;
            }
            else
            {
                return moveState;
            }
        }
        else
        {
            if (_isAnimationFinished)
            {
                if (_isPressingCrouch)
                {
                    return crouchIdleState;
                }
                else
                {
                    return idleState;
                }
            }
        }

        return null;
    }
}
