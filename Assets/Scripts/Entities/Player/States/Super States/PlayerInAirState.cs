using UnityEngine;

[CreateAssetMenu(fileName = "Player In Air State", menuName = "States/Player/In Air/In Air State")]

public class PlayerInAirState : PlayerState
{
    [SerializeField] protected PlayerAttackState primaryAttackState;
    [SerializeField] protected PlayerAttackState secondaryAttackState;
    [SerializeField] protected PlayerJumpState jumpState;
    [SerializeField] protected PlayerCrouchJumpState crouchJumpState;
    [SerializeField] protected PlayerCrouchInAirState crouchInAirState;
    [SerializeField] protected PlayerWallGrabState wallGrabState;
    [SerializeField] protected PlayerWallJumpState wallJumpState;
    [SerializeField] protected PlayerWallSlideState wallSlideState;
    [SerializeField] protected PlayerLandState landState;
    [SerializeField] protected PlayerCrouchLandState crouchLandState;
    [SerializeField] protected PlayerLedgeClimbState ledgeClimbState;

    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.StartCoyoteTime();
    }

    public override void DoActions()
    {
        base.DoActions();

        _temporaryComponent.UpdateFallStatus();
        _temporaryComponent.UpdateCoyoteTime();
        _temporaryComponent.UpdateJumpStatus();
        _temporaryComponent.UpdateMovementDirection();
        _temporaryComponent.MoveInAir();
    }
    
    public override GenericState DoTransitions()
    {
        if (_conditions.IsPressingPrimaryAttack && _conditions.CanAttack && !_conditions.IsReloading)
        {
            return primaryAttackState;
        }
        else if (_conditions.IsPressingSecondaryAttack)
        {
            return null; //return secondaryAttackState;
        }
        else if (_conditions.IsPressingJump && _conditions.CanJump)
        {
            if (_conditions.IsPressingCrouch)
            {
                return crouchJumpState;
            }
            else if (_conditions.IsTouchingWall || _conditions.IsTouchingWall || _conditions.IsInCoyoteTime)
            {
                return wallJumpState;
            }
            else
            {
                return jumpState;
            }
        }
        else if (_conditions.IsGrounded && _conditions.HasStoppedFalling)
        {
            if (_conditions.IsPressingCrouch)
            {
                return crouchLandState;
            }
            else
            {
                return landState;
            }
        }
        else if (_conditions.IsTouchingWall && !_conditions.IsPressingCrouch)
        {
            if (_conditions.IsPressingGrab && _conditions.IsTouchingLedgeHorizontal)
            {
                return wallGrabState;
            }
            else if (_conditions.IsMovingInCorrectDir && _conditions.HasStoppedFalling)
            {
                return wallSlideState;
            }
            else if (!_conditions.IsTouchingLedgeHorizontal && !_conditions.IsGrounded && !_conditions.IsTouchingCeiling)
            {
                return ledgeClimbState;
            }
        }
        else if (_conditions.IsPressingCrouch && _conditions.CanCrouch && !_conditions.IsGrounded)
        {
            return crouchInAirState;
        }

        return null;
    }
}