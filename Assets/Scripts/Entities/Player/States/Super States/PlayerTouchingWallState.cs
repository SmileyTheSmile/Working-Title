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
        if (_stats.IsPressingJump)
        {
            return wallJumpState;
        }
        else if (_stats.IsGrounded && !_stats.IsPressingGrab)
        {
            return idleState;
        }
        else if (_stats.IsGrounded && _stats.IsPressingCrouch && !_stats.IsTouchingCeiling)
        {
            return crouchIdleState;
        }
        else if (!_stats.IsTouchingWall || (!_stats.IsMovingInCorrectDir && !_stats.IsPressingGrab))
        {
            return inAirState;
        }
        else if (_stats.IsTouchingWall && !_stats.IsTouchingLedgeHorizontal && !_stats.IsTouchingCeiling)
        {
            return ledgeClimbState;
        }

        return null;
    }
}
