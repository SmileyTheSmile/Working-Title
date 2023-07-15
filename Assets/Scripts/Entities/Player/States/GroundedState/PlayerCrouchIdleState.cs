using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Idle State", menuName = "States/Player/Grounded/Crouch Idle State")]

public class PlayerCrouchIdleState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerIdleState idleState;

    public override void Enter()
    {
        base.Enter();

        _movement.SetVelocityZero();

        _temporaryComponent.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _conditions.IsPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();

        _temporaryComponent.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _conditions.IsPressingCrouch);
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_conditions.IsMovingX) {
            if (_conditions.IsPressingCrouch || _conditions.IsTouchingCeiling) {
                return crouchMoveState;
            } else {
                return moveState;
            }
        } else if (!_conditions.IsPressingCrouch && !_conditions.IsTouchingCeiling) {
            return idleState;
        }

        return null;
    }
}
