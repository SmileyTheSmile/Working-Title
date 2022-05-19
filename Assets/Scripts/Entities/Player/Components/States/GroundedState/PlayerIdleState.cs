using UnityEngine;

[CreateAssetMenu(fileName = "Player Idle State", menuName = "States/Player/Grounded/Idle State")]

public class PlayerIdleState : PlayerGroundedState
{
    protected PlayerCrouchMoveState crouchMoveState => conditionManager.crouchMoveState;
    protected PlayerMoveState moveState => conditionManager.moveState;
    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    
    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityX(0f);
    }

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
        else if (_isPressingCrouch)
        {
            return crouchIdleState;
        }

        return null;
    }
}
