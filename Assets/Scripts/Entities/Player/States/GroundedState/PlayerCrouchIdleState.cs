using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Idle State", menuName = "States/Player/Grounded/Crouch Idle State")]

public class PlayerCrouchIdleState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerIdleState idleState;

    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.StopMoving();
        _temporaryComponent.Crouch();
    }

    public override void Exit()
    {
        base.Exit();

        _temporaryComponent.UnCrouch();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_conditions.IsMovingX)
        {
            if (_conditions.IsPressingCrouch || _conditions.IsTouchingCeiling)
            {
                return crouchMoveState;
            }
            else
            {
                return moveState;
            }
        }
        else if (!_conditions.IsPressingCrouch && !_conditions.IsTouchingCeiling)
        {
            return idleState;
        }

        return null;
    }
}
