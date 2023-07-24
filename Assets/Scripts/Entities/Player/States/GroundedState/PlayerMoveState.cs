using UnityEngine;

[CreateAssetMenu(fileName = "Player Move State", menuName = "States/Player/Grounded/Move State")]

public class PlayerMoveState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;

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

        _temporaryComponent.MoveOnGround();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (!_conditions.IsMovingX)
        {
            if (_conditions.IsPressingCrouch)
            {
                return crouchIdleState;
            }
            else
            {
                return idleState;
            }
        }
        else if (_conditions.IsPressingCrouch) {
            return crouchMoveState;
        }

        return null;
    }
}