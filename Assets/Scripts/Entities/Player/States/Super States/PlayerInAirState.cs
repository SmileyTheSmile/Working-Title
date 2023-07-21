using UnityEngine;

[CreateAssetMenu(fileName = "Player In Air State", menuName = "States/Player/In Air/In Air State")]

public class PlayerInAirState : PlayerState
{
    protected Movement _movement;
    protected SoundComponent _sound;

    [SerializeField] protected PlayerConditionTable _conditions;

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

    private float _airControlPercentage => _playerData.defaultAirControlPercentage;
    private bool _coyoteTime;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _movement = _core.GetCoreComponent<Movement>();
        _sound = _core.GetCoreComponent<SoundComponent>();
    }

    public override void Enter()
    {
        base.Enter();

        if (_sound.jumpSound)
            _sound.jumpSound.Play();

        StartCoyoteTime();
    }

    public override void DoActions()
    {
        base.DoActions();

        CheckJumpMultiplier();
        CheckCoyoteTime();

        _temporaryComponent.CheckMovementDirection(_conditions.NormalizedInputX);
        
        if (_conditions.NormalizedInputX != 0)
            _movement.SetVelocityX(_playerData.movementVelocity * _conditions.NormalizedInputX * _airControlPercentage);

        _visualController?.SetAnimationFloat("velocityX", Mathf.Abs(_movement.CurrentVelocity.x));
        _visualController?.SetAnimationFloat("velocityY", _movement.CurrentVelocity.y);
    }
    
    public override GenericState DoTransitions()
    {
        if (_conditions.IsPressingPrimaryAttack && _conditions.CanAttack && !_conditions.IsReloading) {
            return primaryAttackState;
        } else if (_conditions.IsPressingSecondaryAttack) {
            return null; //return secondaryAttackState;
        } else if (_conditions.IsPressingJump && _conditions.CanJump) {
            if (_conditions.IsPressingCrouch) {
                return crouchJumpState;
            } else if (_conditions.IsTouchingWall || _conditions.IsTouchingWall || _coyoteTime) {
                return wallJumpState;
            } else {
                return jumpState;
            }
        } else if (_conditions.IsGrounded && _conditions.HasStoppedFalling) {
            if (_conditions.IsPressingCrouch) {
                return crouchLandState;
            } else {
                return landState;
            }
        } else if (_conditions.IsTouchingWall && !_conditions.IsPressingCrouch) {
            if (_conditions.IsPressingGrab && _conditions.IsTouchingLedgeHorizontal) {
                return wallGrabState;
            } else if (_conditions.IsMovingInCorrectDir && _conditions.HasStoppedFalling) {
                return wallSlideState;
            } else if (!_conditions.IsTouchingLedgeHorizontal && !_conditions.IsGrounded && !_conditions.IsTouchingCeiling) {
                return ledgeClimbState;
            }
        } else if (_conditions.IsPressingCrouch && _conditions.CanCrouch && !_conditions.IsGrounded) {
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
        if (!_conditions.IsJumping)
            return;

        if (_conditions.IsJumpCanceled) {
            _movement.SetVelocityY(_movement.CurrentVelocity.y * _playerData.variableJumpHeightMultiplier);
            _conditions.IsJumping = false;
        } else if (_movement.CurrentVelocity.y <= 0f) {
            _conditions.IsJumping = false;
        }
    }

    private void StartCoyoteTime() => _coyoteTime = true;
}