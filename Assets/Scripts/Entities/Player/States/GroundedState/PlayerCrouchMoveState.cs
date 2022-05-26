using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Move State", menuName = "States/Player/Grounded/Crouch Move State")]

public class PlayerCrouchMoveState : PlayerGroundedState
{
    protected PlayerMoveState moveState => conditionManager.moveState;
    protected PlayerIdleState idleState => conditionManager.idleState;
    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    
    private int _movementDir => conditionManager._movementDirSO.value;

    protected AudioSourcePlayer _moveSound => conditionManager.moveSound;
    protected float _lastStepTime;
    protected float _stepDelay => conditionManager.stepDelay;

    public override void Enter()
    {
        base.Enter();

        Step();

        movement.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    private void Step()
    {
        _lastStepTime = Time.time;

        if (_moveSound)
        {
            _moveSound.Play();
        }
    }

    public override void Exit()
    {
        base.Exit();

        _moveSound.Stop();

        movement.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void DoActions()
    {
        base.DoActions();

        if (_lastStepTime + _stepDelay < Time.time)
        {
            Step();
        }

        movement.SetVelocityX(_playerData.crouchMovementVelocity * _movementDir);
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
