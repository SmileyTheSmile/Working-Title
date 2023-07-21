using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Move State", menuName = "States/Player/Grounded/Crouch Move State")]

public class PlayerCrouchMoveState : PlayerGroundedState
{
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerIdleState idleState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;

    protected float _stepDelay => _temporaryComponent.stepDelay;
    protected float _lastStepTime;

    public override void Enter()
    {
        base.Enter();

        Step();

        _temporaryComponent.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _conditions.IsPressingCrouch);
    }

    private void Step()
    {
        _lastStepTime = Time.time;

        if (_sound.moveSound)
            _sound.moveSound.Play();
    }

    public override void Exit()
    {
        base.Exit();

        _sound.moveSound.Stop();

        _temporaryComponent.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _conditions.IsPressingCrouch);
    }

    public override void DoActions()
    {
        base.DoActions();

        if (_lastStepTime + _stepDelay < Time.time)
        {
            Step();
        }

        _movement.SetVelocityX(_playerData.crouchMovementVelocity * _conditions.MovementDir);
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult != null)
            return parentResult;

        if (_conditions.IsMovingX)
        {
            if (!_conditions.IsPressingCrouch && !_conditions.IsTouchingCeiling) {
                return moveState;
            }
        } else {
            if (!_conditions.IsPressingCrouch && !_conditions.IsTouchingCeiling) {
                return idleState;
            } else {
                return crouchIdleState;
            }
        }

        return null;
    }
}
