using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Climb State", menuName = "States/Player/Touching Wall/Wall Climb State")]

public class PlayerWallClimbState : PlayerTouchingWallState
{
    [SerializeField] protected PlayerWallGrabState wallGrabState;

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

        _sound.moveSound.Stop();
    }

    public override void DoActions()
    {
        base.DoActions();

        if (_lastStepTime + _stepDelay < Time.time)
        {
            Step();
        }

        _movement.SetVelocityY(_playerData.wallClimbVelocity);
    }

    private void Step()
    {
        _lastStepTime = Time.time;

        if (_sound.moveSound)
            _sound.moveSound.Play();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();

        if (parentResult != null)
        {
            return parentResult;
        }

        if (!_conditions.IsMovingUp)
        {
            return wallGrabState;
        }

        return null;
    }
}
