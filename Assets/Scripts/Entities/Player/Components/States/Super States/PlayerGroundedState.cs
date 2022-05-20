using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
    protected Movement movement
    { get => _movement ?? _core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected int _inputX => conditionManager._normalizedInputXSO.value;

    protected bool _isPressingGrab => conditionManager.IsPressingGrabSO.value;
    protected bool _isPressingCrouch => conditionManager.IsPressingCrouchSO.value;
    protected bool _isPressingJump => conditionManager.IsPressingJumpSO.value;
    protected bool _isPressingPrimaryAttack => conditionManager.IsPressingPrimaryAttackSO.value;
    protected bool _isPressingSecondaryAttack => conditionManager.IsPressingSecondaryAttackSO.value;
    protected bool _isMovingX => conditionManager.IsMovingXSO.value;

    protected bool _isGrounded => conditionManager.IsGroundedSO.value;
    protected bool _isTouchingCeiling => conditionManager.IsTouchingCeilingSO.value;
    protected bool _isTouchingWall => conditionManager.IsTouchingWallFrontSO.value;
    protected bool _isTouchingLedge => conditionManager.IsTouchingLedgeHorizontalSO.value;

    protected bool _canCrouch => conditionManager.CanCrouchSO.value;
    protected bool _canJump => conditionManager.CanJumpSO.value;
    protected bool _isJumping => conditionManager.IsJumpingSO.value;

    protected PlayerAttackState primaryAttackState => conditionManager.primaryAttackState;
    protected PlayerAttackState secondaryAttackState => conditionManager.secondaryAttackState;
    protected PlayerJumpState jumpState => conditionManager.jumpState;
    protected PlayerCrouchJumpState crouchJumpState => conditionManager.crouchJumpState;
    protected PlayerInAirState inAirState => conditionManager.inAirState;
    protected PlayerCrouchInAirState crouchInAirState => conditionManager.crouchInAirState;
    protected PlayerWallGrabState wallGrabState => conditionManager.wallGrabState;

    public override void Enter()
    {
        base.Enter();

        conditionManager.ResetAmountOfJumpsLeft();
        conditionManager.ResetAmountOfCrouchesLeft();
    }

    public override void DoActions()
    {
        base.DoActions();

        movement.CheckMovementDirection(_inputX);
    }

    public override GenericState DoTransitions()
    {
        if (_isPressingPrimaryAttack && !_isTouchingCeiling)
        {
            //return primaryAttackState;
        }
        else if (_isPressingSecondaryAttack && !_isTouchingCeiling)
        {
            return secondaryAttackState;
        }
        else if (((_isPressingJump || _isJumping) && _canJump && !_isTouchingCeiling))
        {
            if (_isPressingCrouch && _canCrouch)
            {
                return crouchJumpState;
            }
            else
            {
                return jumpState;
            }
        }
        else if (!_isGrounded)
        {
            if (_isPressingCrouch)
            {
                return crouchInAirState;
            }
            else
            {
                return inAirState;
            }
        }
        else if (_isTouchingWall && _isPressingGrab && _isTouchingLedge && !_isTouchingCeiling && !_isPressingCrouch)
        {
            return wallGrabState;
        }

        return null;
    }
}