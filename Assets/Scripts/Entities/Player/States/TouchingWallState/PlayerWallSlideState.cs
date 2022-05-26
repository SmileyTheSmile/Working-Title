using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Slide State", menuName = "States/Player/Touching Wall/Wall Slide State")]

public class PlayerWallSlideState : PlayerTouchingWallState
{
    protected PlayerWallGrabState wallGrabState => conditionManager.wallGrabState;

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

        movement.SetVelocityY(-_playerData.wallSlideVelocity);
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

        if (_isPressingGrab && !_isMovingUp && !_isMovingDown)
        {
            return wallGrabState;
        }

        return null;
    }
}