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

        _player.StartCoyoteTime();
    }

    public override void DoActions()
    {
        base.DoActions();

        _player.UpdateFallStatus();
        _player.UpdateCoyoteTime();
        _player.UpdateJumpStatus();
        _player.UpdateMovementDirection();
        _player.MoveInAir();
    }
    
    public override GenericState DoTransitions()
    {
        if (_stats.IsPressingPrimaryAttack && _stats.CanAttack && !_stats.IsReloading)
        {
            return primaryAttackState;
        }
        else if (_stats.IsPressingSecondaryAttack)
        {
            return null; //return secondaryAttackState;
        }
        else if (_stats.IsPressingJump && _stats.CanJump)
        {
            if (_stats.IsPressingCrouch)
            {
                return crouchJumpState;
            }
            else if (_stats.IsTouchingWall || _stats.IsTouchingWall || _stats.IsInCoyoteTime)
            {
                return wallJumpState;
            }
            else
            {
                return jumpState;
            }
        }
        else if (_stats.IsGrounded && _stats.HasStoppedFalling)
        {
            if (_stats.IsPressingCrouch)
            {
                return crouchLandState;
            }
            else
            {
                return landState;
            }
        }
        else if (_stats.IsTouchingWall && !_stats.IsPressingCrouch)
        {
            if (_stats.IsPressingGrab && _stats.IsTouchingLedgeHorizontal)
            {
                return wallGrabState;
            }
            else if (_stats.IsMovingInCorrectDir && _stats.HasStoppedFalling)
            {
                return wallSlideState;
            }
            else if (!_stats.IsTouchingLedgeHorizontal && !_stats.IsGrounded && !_stats.IsTouchingCeiling)
            {
                return ledgeClimbState;
            }
        }
        else if (_stats.IsPressingCrouch && _stats.CanCrouch && !_stats.IsGrounded)
        {
            return crouchInAirState;
        }

        return null;
    }
}