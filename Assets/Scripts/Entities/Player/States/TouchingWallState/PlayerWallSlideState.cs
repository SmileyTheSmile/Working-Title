using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Slide State", menuName = "States/Player/Touching Wall/Wall Slide State")]

public class PlayerWallSlideState : PlayerTouchingWallState
{
    [SerializeField] protected PlayerWallGrabState wallGrabState;

    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.Step();
    }

    public override void Exit()
    {
        base.Exit();

        _temporaryComponent.StopMovementSound();
    }
    
    public override void DoActions()
    {
        base.DoActions();

        _temporaryComponent.SlideDown();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_conditions.IsPressingGrab && !_conditions.IsMovingUp && !_conditions.IsMovingDown)
        {
            return wallGrabState;
        }

        return null;
    }
}