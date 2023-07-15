using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Land State", menuName = "States/Player/Grounded/Crouch Land State")]

public class PlayerCrouchLandState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;

    protected AudioSourcePlayer _fallSound => _temporaryComponent.fallSound;

    public override void Enter()
    {
        base.Enter();

        if (_fallSound)
            _fallSound.Play();

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
            if (_conditions.IsPressingCrouch) {
                return crouchMoveState;
            } else {
                return moveState;
            }
        } else if (_isAnimationFinished) {
            if (_conditions.IsPressingCrouch) {
                return crouchIdleState;
            } else {
                return idleState;
            }
        }

        return null;
    }
}
