using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
    [SerializeField] protected PlayerAttackState primaryAttackState;
    [SerializeField] protected PlayerAttackState secondaryAttackState;
    [SerializeField] protected PlayerJumpState jumpState;
    [SerializeField] protected PlayerCrouchJumpState crouchJumpState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] protected PlayerCrouchInAirState crouchInAirState;
    [SerializeField] protected PlayerWallGrabState wallGrabState;

    public override void DoActions()
    {
        base.DoActions();

        _player.UpdateMovementDirection();
    }

    public override GenericState DoTransitions()
    {
        if (_stats.IsPressingPrimaryAttack && !_stats.IsTouchingCeiling && _stats.CanAttack && !_stats.IsReloading)
        {
            return primaryAttackState;
        }
        else if (_stats.IsPressingSecondaryAttack && !_stats.IsTouchingCeiling)
        {
            return null; //secondaryAttackState;
        }
        else if ((_stats.IsPressingJump && !_stats.IsJumping) && _stats.CanJump && !_stats.IsTouchingCeiling)
        {
            if (_stats.IsPressingCrouch && _stats.CanCrouch)
            {
                return crouchJumpState;
            }
            else
            {
                return jumpState;
            }
        }
        else if (!_stats.IsGrounded)
        {
            if (_stats.IsPressingCrouch)
            {
                return crouchInAirState;
            }
            else
            {
                return inAirState;
            }
        } else if (_stats.IsTouchingWall && _stats.IsPressingGrab && _stats.IsTouchingLedgeHorizontal && !_stats.IsTouchingCeiling && !_stats.IsPressingCrouch)
        {
            return wallGrabState;
        }

        return null;
    }
}