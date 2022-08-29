using UnityEngine;

[CreateAssetMenu(fileName = "Player In Air State", menuName = "States/Player/In Air/In Air State")]

public class PlayerInAirState : PlayerState
{
    protected Movement _movement;

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

    [SerializeField] protected ScriptableInt InputXSO;

    [SerializeField] protected InputTransitionCondition IsPressingGrabSO;
    [SerializeField] protected InputTransitionCondition IsPressingCrouchSO;
    [SerializeField] protected InputTransitionCondition IsPressingJumpSO;
    [SerializeField] protected InputTransitionCondition IsJumpCanceledSO;
    [SerializeField] protected InputTransitionCondition IsPressingPrimaryAttackSO;
    [SerializeField] protected InputTransitionCondition IsPressingSecondaryAttackSO;

    [SerializeField] protected CollisionCheckTransitionCondition IsGroundedSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingWallFrontSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingWallBackSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingLedgeHorizontalSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingCeilingSO;

    [SerializeField] protected SupportTransitionCondition IsJumpingSO;
    [SerializeField] protected SupportTransitionCondition CanJumpSO;
    [SerializeField] protected SupportTransitionCondition CanCrouchSO;
    [SerializeField] protected SupportTransitionCondition HasStoppedFalling;
    [SerializeField] protected SupportTransitionCondition IsMovingInCorrectDirSO;
    [SerializeField] protected SupportTransitionCondition CanAttackSO;
    [SerializeField] protected SupportTransitionCondition IsReloadingSO;

    protected int _inputX => InputXSO.value;

    protected bool _isPressingGrab => IsPressingGrabSO.value;
    protected bool _isPressingCrouch => IsPressingCrouchSO.value;
    protected bool _isPressingJump => IsPressingJumpSO.value;
    protected bool _isJumpCanceled => IsJumpCanceledSO.value;
    protected bool _isPressingPrimaryAttack => IsPressingPrimaryAttackSO.value;
    protected bool _isPressingSecondaryAttack => IsPressingSecondaryAttackSO.value;

    protected bool _isGrounded => IsGroundedSO.value;
    protected bool _isTouchingWall => IsTouchingWallFrontSO.value;
    protected bool _isTouchingWallBack => IsTouchingWallBackSO.value;
    protected bool _isTouchingLedge => IsTouchingLedgeHorizontalSO.value;
    protected bool _isTouchingCeiling => IsTouchingCeilingSO.value;

    protected bool _canJump => CanJumpSO.value;
    protected bool _canCrouch => CanCrouchSO.value;
    protected bool _hasStoppedFalling => HasStoppedFalling.value;
    protected bool _isMovingInCorrectDir => IsMovingInCorrectDirSO.value;
    protected bool _canAttack => CanAttackSO.value;
    protected bool _isReloading => IsReloadingSO.value;

    protected AudioSourcePlayer _jumpSound => _temporaryComponent.jumpSound;

    private float _airControlPercentage => _playerData.defaultAirControlPercentage;
    private bool _coyoteTime;

    public override void Initialize(EntityCore entity)
    {
        base.Initialize(entity);

        _movement = _core.GetCoreComponent<Movement>();
    }

    public override void Enter()
    {
        base.Enter();

        if (_jumpSound)
            _jumpSound.Play();

        StartCoyoteTime();
    }

    public override void DoActions()
    {
        base.DoActions();

        CheckJumpMultiplier();
        CheckCoyoteTime();

        _temporaryComponent.CheckMovementDirection(_inputX);
        
        if (_inputX != 0)
            _movement.SetVelocityX(_playerData.movementVelocity * _inputX * _airControlPercentage);

        _visualController?.SetAnimationFloat("velocityX", Mathf.Abs(_movement.CurrentVelocity.x));
        _visualController?.SetAnimationFloat("velocityY", _movement.CurrentVelocity.y);
    }
    
    public override GenericState DoTransitions()
    {
        if (_isPressingPrimaryAttack && _canAttack && !_isReloading)
        {
            return primaryAttackState;
        }
        else if (_isPressingSecondaryAttack)
        {
            return null;
            //return secondaryAttackState;
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
            _temporaryComponent.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckJumpMultiplier()
    {
        if (!IsJumpingSO.value)
            return;

        if (_isJumpCanceled)
        {
            _movement.SetVelocityY(_movement.CurrentVelocity.y * _playerData.variableJumpHeightMultiplier);
            IsJumpingSO.value = false;
        }
        else if (_movement.CurrentVelocity.y <= 0f)
        {
            IsJumpingSO.value = false;
        }
    }

    private void StartCoyoteTime() => _coyoteTime = true;
}