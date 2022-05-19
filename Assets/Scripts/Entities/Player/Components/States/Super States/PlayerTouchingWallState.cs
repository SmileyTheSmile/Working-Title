using UnityEngine;

public abstract class PlayerTouchingWallState : PlayerState
{
    protected Movement movement
    { get => _movement ?? _core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected int _inputX => inputHandler._normalizedInputXSO.value;
    protected int _inputY => inputHandler._normalizedInputYSO.value;

    protected bool _isPressingGrab => conditionManager.IsPressingGrabSO.value;
    protected bool _isPressingJump => conditionManager.IsPressingJumpSO.value;
    protected bool _isPressingCrouch => conditionManager.IsPressingCrouchSO.value;

    protected bool _isGrounded => conditionManager.IsGroundedSO.value;
    protected bool _isTouchingWall => conditionManager.IsTouchingWallFrontSO.value;
    protected bool _isTouchingLedge => conditionManager.IsTouchingLedgeHorizontalSO.value;
    protected bool _isTouchingCeiling => conditionManager.IsTouchingCeilingSO.value;

    protected bool _isMovingUp => conditionManager.IsMovingUpSO.value;
    protected bool _isMovingDown => conditionManager.IsMovingDownSO.value;
    protected bool _isMovingInCorrectDir => conditionManager.IsMovingInCorrectDirSO.value;
    
    protected PlayerIdleState idleState => conditionManager.idleState;
    protected PlayerInAirState inAirState => conditionManager.inAirState;
    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    protected PlayerWallJumpState wallJumpState => conditionManager.wallJumpState;
    protected PlayerLedgeClimbState ledgeClimbState => conditionManager.ledgeClimbState;
    
    public override GenericState DoTransitions()
    {
        if (_isPressingJump)
        {
            return wallJumpState;
        }
        else if (_isGrounded && !_isPressingGrab)
        {
            return idleState;
        }
        else if (_isGrounded && _isPressingCrouch && !_isTouchingCeiling)
        {
            return crouchIdleState;
        }
        else if (!_isTouchingWall || (!_isMovingInCorrectDir && !_isPressingGrab))
        {
            return inAirState;
        }
        else if (_isTouchingWall && !_isTouchingLedge && !_isTouchingCeiling)
        {
            return ledgeClimbState;
        }

        return null;
    }
}
