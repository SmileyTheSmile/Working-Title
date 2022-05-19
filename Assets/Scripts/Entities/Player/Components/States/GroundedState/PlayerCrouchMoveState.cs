using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Move State", menuName = "States/Player/Grounded/Crouch Move State")]

public class PlayerCrouchMoveState : PlayerGroundedState
{
    protected PlayerMoveState moveState => conditionManager.moveState;
    protected PlayerIdleState idleState => conditionManager.idleState;
    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    
    public override void Enter()
    {
        base.Enter();

        movement?.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();

        movement?.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void DoActions()
    {
        base.DoActions();

        movement?.SetVelocityX(_playerData.crouchMovementVelocity * movement._movementDir);
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
            if (!_isPressingCrouch && !_isTouchingCeiling)
            {
                return moveState;
            }
        }
        else
        {
            if (!_isPressingCrouch && !_isTouchingCeiling)
            {
                return idleState;
            }
            else
            {
                return crouchIdleState;
            }
        }

        return null;
    }
}
