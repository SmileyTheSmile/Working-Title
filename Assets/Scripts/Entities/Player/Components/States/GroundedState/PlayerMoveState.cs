using UnityEngine;

[CreateAssetMenu(fileName = "Player Move State", menuName = "States/Player/Grounded/Move State")]

public class PlayerMoveState : PlayerGroundedState
{
    protected PlayerCrouchMoveState crouchMoveState => conditionManager.crouchMoveState;
    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    protected PlayerIdleState idleState => conditionManager.idleState;

    public override void DoActions()
    {
        base.DoActions();

        movement?.SetVelocityX(_playerData.movementVelocity * _inputX);
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();

        if (parentResult != null)
        {
            return parentResult;
        }

        if (!_isMovingX)
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
        else if (_isPressingCrouch)
        {
            return crouchMoveState;
        }

        return null;
    }
}