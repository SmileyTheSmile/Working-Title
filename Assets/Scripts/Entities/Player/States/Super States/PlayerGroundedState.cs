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

        _temporaryComponent.UpdateMovementDirection();
    }

    public override GenericState DoTransitions()
    {
        if (_conditions.IsPressingPrimaryAttack && !_conditions.IsTouchingCeiling && _conditions.CanAttack && !_conditions.IsReloading)
        {
            return primaryAttackState;
        }
        else if (_conditions.IsPressingSecondaryAttack && !_conditions.IsTouchingCeiling)
        {
            return null; //secondaryAttackState;
        }
        else if ((_conditions.IsPressingJump && _conditions.IsJumping) && _conditions.CanJump && !_conditions.IsTouchingCeiling)
        {
            if (_conditions.IsPressingCrouch && _conditions.CanCrouch)
            {
                return crouchJumpState;
            }
            else
            {
                return jumpState;
            }
        }
        else if (!_conditions.IsGrounded)
        {
            if (_conditions.IsPressingCrouch)
            {
                return crouchInAirState;
            }
            else
            {
                return inAirState;
            }
        } else if (_conditions.IsTouchingWall && _conditions.IsPressingGrab && _conditions.IsTouchingLedgeHorizontal && !_conditions.IsTouchingCeiling && !_conditions.IsPressingCrouch)
        {
            return wallGrabState;
        }

        return null;
    }
}