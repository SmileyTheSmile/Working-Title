using UnityEngine;

[CreateAssetMenu(fileName = "Player In Air State", menuName = "States/Player/In Air/In Air State")]

public class PlayerInAirState : PlayerState
{
    protected Movement movement
    { get => _movement ?? _entity.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected int _inputX => conditionManager._normalizedInputXSO.value;

    protected bool _isPressingGrab => conditionManager.IsPressingGrabSO.value;
    protected bool _isPressingCrouch => conditionManager.IsPressingCrouchSO.value;
    protected bool _isPressingJump => conditionManager.IsPressingJumpSO.value;
    protected bool _isJumpCanceled => conditionManager.IsJumpCanceledSO.value;
    protected bool _isPressingPrimaryAttack => conditionManager.IsPressingPrimaryAttackSO.value;
    protected bool _isPressingSecondaryAttack => conditionManager.IsPressingSecondaryAttackSO.value;

    protected bool _isGrounded => conditionManager.IsGroundedSO.value;
    protected bool _isTouchingWall => conditionManager.IsTouchingWallFrontSO.value;
    protected bool _isTouchingWallBack => conditionManager.IsTouchingWallBackSO.value;
    protected bool _isTouchingLedge => conditionManager.IsTouchingLedgeHorizontalSO.value;
    protected bool _isTouchingCeiling => conditionManager.IsTouchingCeilingSO.value;

    protected bool _isJumping  { get => conditionManager.IsJumpingSO.value; set => conditionManager.IsJumpingSO.value = value;}
    protected bool _canJump => conditionManager.CanJumpSO.value;
    protected bool _canCrouch => conditionManager.CanCrouchSO.value;
    protected bool _hasStoppedFalling => conditionManager.HasStoppedFalling.value;
    protected bool _isMovingInCorrectDir => conditionManager.IsMovingInCorrectDirSO.value;

    private float _airControlPercentage => _playerData.defaultAirControlPercentage;
    private bool _coyoteTime;

    protected PlayerAttackState primaryAttackState => conditionManager.primaryAttackState;
    protected PlayerAttackState secondaryAttackState => conditionManager.secondaryAttackState;
    protected PlayerJumpState jumpState => conditionManager.jumpState;
    protected PlayerCrouchJumpState crouchJumpState => conditionManager.crouchJumpState;
    protected PlayerCrouchInAirState crouchInAirState => conditionManager.crouchInAirState;
    protected PlayerWallGrabState wallGrabState => conditionManager.wallGrabState;
    protected PlayerWallJumpState wallJumpState => conditionManager.wallJumpState;
    protected PlayerWallSlideState wallSlideState => conditionManager.wallSlideState;
    protected PlayerLandState landState => conditionManager.landState;
    protected PlayerCrouchLandState crouchLandState => conditionManager.crouchLandState;
    protected PlayerLedgeClimbState ledgeClimbState => conditionManager.ledgeClimbState;

    public override void Enter()
    {
        base.Enter();

        StartCoyoteTime();
    }

    public override void DoActions()
    {
        base.DoActions();

        CheckJumpMultiplier();
        CheckCoyoteTime();

        movement.CheckMovementDirection(_inputX);
        
        if (_inputX != 0)
            movement.SetVelocityX(_playerData.movementVelocity * _inputX * _airControlPercentage);

        visualController?.SetAnimationFloat("velocityX", Mathf.Abs(movement.currentVelocity.x));
        visualController?.SetAnimationFloat("velocityY", movement.currentVelocity.y);
    }
    
    public override GenericState DoTransitions()
    {
        if (_isPressingPrimaryAttack)
        {
            //stateMachine?.ChangeState(player.primaryAttackState);
        }
        else if (_isPressingSecondaryAttack)
        {
            return secondaryAttackState;
        }
        else if (_isPressingJump && _canJump)
        {
            if (_isPressingCrouch)
            {
                return crouchJumpState;
            }
            else if (_isTouchingWall || _isTouchingWallBack || _coyoteTime)
            {
                return wallJumpState;
            }
            else
            {
                return jumpState;
            }
        }
        else if (_isGrounded && _hasStoppedFalling)
        {
            if (_isPressingCrouch)
            {
                return crouchLandState;
            }
            else
            {
                return landState;
            }
        }
        else if (_isTouchingWall && !_isPressingCrouch)
        {
            if (_isPressingGrab && _isTouchingLedge)
            {
                return wallGrabState;
            }
            else if (_isMovingInCorrectDir && _hasStoppedFalling)
            {
                return wallSlideState;
            }
            else if (!_isTouchingLedge && !_isGrounded && !_isTouchingCeiling)
            {
                return ledgeClimbState;
            }
        }
        else if (_isPressingCrouch && _canCrouch && !_isGrounded)
        {
            return crouchInAirState;
        }

        return null;
    }

    private void CheckCoyoteTime()
    {
        if (_coyoteTime && Time.time > _startTime + _playerData.coyoteTime)
        {
            _coyoteTime = false;
            conditionManager.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckJumpMultiplier()
    {
        if (!_isJumping)
        {
            return;
        }

        if (_isJumpCanceled)
        {
            movement.SetVelocityY(movement.currentVelocity.y * _playerData.variableJumpHeightMultiplier);
            _isJumping = false;
        }
        else if (movement.currentVelocity.y <= 0f)
        {
            _isJumping = false;
        }
    }

    private void StartCoyoteTime() => _coyoteTime = true;
}