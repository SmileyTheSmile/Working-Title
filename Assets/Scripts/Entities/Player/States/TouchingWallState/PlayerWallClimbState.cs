using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Climb State", menuName = "States/Player/Touching Wall/Wall Climb State")]

public class PlayerWallClimbState : PlayerTouchingWallState
{
    [SerializeField] protected PlayerWallGrabState wallGrabState;

    protected AudioSourcePlayer _moveSound => conditionManager.moveSound;
    protected float _lastStepTime;
    protected float _stepDelay => conditionManager.stepDelay;

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
        {
            Step();
        }

        movement.SetVelocityY(_playerData.wallClimbVelocity);
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

        if (!_isMovingUp)
        {
            return wallGrabState;
        }

        return null;
    }
}
