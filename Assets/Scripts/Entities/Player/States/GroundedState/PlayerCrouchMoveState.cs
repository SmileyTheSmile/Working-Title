using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Move State", menuName = "States/Player/Grounded/Crouch Move State")]

public class PlayerCrouchMoveState : PlayerGroundedState
{
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerIdleState idleState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;

    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.Step();
        _temporaryComponent.Crouch();
    }

    public override void Exit()
    {
        base.Exit();

        _temporaryComponent.StopMovementSound();
        _temporaryComponent.UnCrouch();
    }

    public override void DoActions()
    {
        base.DoActions();

        _temporaryComponent.MoveOnGroundCrouched();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_conditions.IsMovingX)
        {
            if (!_conditions.IsPressingCrouch && !_conditions.IsTouchingCeiling)
            {
                return moveState;
            }
        }
        else
        {
            if (!_conditions.IsPressingCrouch && !_conditions.IsTouchingCeiling)
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
