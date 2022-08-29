using UnityEngine;

[CreateAssetMenu(fileName = "Player Move State", menuName = "States/Player/Grounded/Move State")]

public class PlayerMoveState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;

    protected AudioSourcePlayer _moveSound => _temporaryComponent.moveSound;
    protected float _lastStepTime;
    protected float _stepDelay => _temporaryComponent.stepDelay;

    public override void Enter()
    {
        base.Enter();

        Step();
    }

    public override void Exit()
    {
        base.Exit();

        _moveSound.Stop();
    }

    public override void DoActions()
    {
        base.DoActions();

        if (_lastStepTime + _stepDelay < Time.time)
            Step();

        _movement.SetVelocityX(_playerData.movementVelocity * _inputX);
        //movement.AddForceX(_playerData.movementVelocity * _inputX, ForceMode2D.Impulse);
    }

    private void Step()
    {
        _lastStepTime = Time.time;

        if (_moveSound)
        {
            _moveSound.Play();
        }
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