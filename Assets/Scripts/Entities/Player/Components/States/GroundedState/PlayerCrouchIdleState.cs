using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Idle State", menuName = "States/Player/Grounded/Crouch Idle State")]

public class PlayerCrouchIdleState : PlayerGroundedState
{
    protected PlayerCrouchMoveState crouchMoveState => conditionManager.crouchMoveState;
    protected PlayerMoveState moveState => conditionManager.moveState;
    protected PlayerIdleState idleState => conditionManager.idleState;

    public override void Enter()
    {
        base.Enter();

        movement.SetVelocityZero();

        movement.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();

        movement.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
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
            if (_isPressingCrouch || _isTouchingCeiling)
            {
                return crouchMoveState;
            }
            else
            {

                return moveState;
            }
        }
        else if (!_isPressingCrouch && !_isTouchingCeiling)
        {
            return idleState;
        }

        return null;
    }
}
