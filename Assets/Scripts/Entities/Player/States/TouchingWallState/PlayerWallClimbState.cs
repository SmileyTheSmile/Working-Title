using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Climb State", menuName = "States/Player/Touching Wall/Wall Climb State")]

public class PlayerWallClimbState : PlayerTouchingWallState
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

        _temporaryComponent.ClimbWall();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (!_conditions.IsMovingUp)
        {
            return wallGrabState;
        }

        return null;
    }
}
