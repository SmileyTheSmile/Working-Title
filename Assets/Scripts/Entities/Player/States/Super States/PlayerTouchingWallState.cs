using UnityEngine;

public abstract class PlayerTouchingWallState : PlayerState
{
    protected Movement _movement;

    [SerializeField] protected PlayerIdleState idleState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerWallJumpState wallJumpState;
    [SerializeField] protected PlayerLedgeClimbState ledgeClimbState;

    [SerializeField] protected InputTransitionCondition IsPressingGrabSO;
    [SerializeField] protected InputTransitionCondition IsPressingCrouchSO;
    [SerializeField] protected InputTransitionCondition IsPressingJumpSO;
    [SerializeField] protected InputTransitionCondition IsMovingUpSO;
    [SerializeField] protected InputTransitionCondition IsMovingDownSO;

    [SerializeField] protected CollisionCheckTransitionCondition IsGroundedSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingWallFrontSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingLedgeHorizontalSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingCeilingSO;
    [SerializeField] protected SupportTransitionCondition IsMovingInCorrectDirSO;

    protected bool _isPressingGrab => IsPressingGrabSO.value;
    protected bool _isPressingJump => IsPressingJumpSO.value;
    protected bool _isPressingCrouch => IsPressingCrouchSO.value;

    protected bool _isGrounded => IsGroundedSO.value;
    protected bool _isTouchingWall => IsTouchingWallFrontSO.value;
    protected bool _isTouchingLedge => IsTouchingLedgeHorizontalSO.value;
    protected bool _isTouchingCeiling => IsTouchingCeilingSO.value;

    protected bool _isMovingUp => IsMovingUpSO.value;
    protected bool _isMovingDown => IsMovingDownSO.value;
    protected bool _isMovingInCorrectDir => IsMovingInCorrectDirSO.value;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _movement = _core.GetCoreComponent<Movement>();
    }

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
