using UnityEngine;

public abstract class PlayerTouchingWallState : PlayerState
{
    [SerializeField] protected PlayerIdleState idleState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerWallJumpState wallJumpState;
    [SerializeField] protected PlayerLedgeClimbState ledgeClimbState;

    public override GenericState DoTransitions()
    {
        if (_conditions.IsPressingJump)
        {
            return wallJumpState;
        }
        else if (_conditions.IsGrounded && !_conditions.IsPressingGrab)
        {
            return idleState;
        }
        else if (_conditions.IsGrounded && _conditions.IsPressingCrouch && !_conditions.IsTouchingCeiling)
        {
            return crouchIdleState;
        }
        else if (!_conditions.IsTouchingWall || (!_conditions.IsMovingInCorrectDir && !_conditions.IsPressingGrab))
        {
            return inAirState;
        }
        else if (_conditions.IsTouchingWall && !_conditions.IsTouchingLedgeHorizontal && !_conditions.IsTouchingCeiling)
        {
            return ledgeClimbState;
        }

        return null;
    }
}
