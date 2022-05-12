using UnityEngine;

[CreateAssetMenu(fileName = "Player In Air State", menuName = "States/Player/In Air/In Air State")]

public class PlayerInAirState : PlayerState
{
    private ConditionManager conditionManager
    { get => _conditionManager ?? core.GetCoreComponent(ref _conditionManager); }
    private ConditionManager _conditionManager;

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected int _inputX => inputHandler.normalizedInputX;
    protected int _inputY => inputHandler.normalizedInputY;
    protected Vector2 _mousePositionInput => inputHandler.mousePositionInput;
    
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
    protected bool _oldIsTouchingWall;
    protected bool _oldIsTouchingWallBack;
    protected bool _isJumping;

    private float _wallJumpCoyoteTimeStart;
    private float _airControlPercentage = 1f;
    private bool _coyoteTime;
    private bool _wallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData)
    {
        _airControlPercentage = playerData.defaultAirControlPercentage;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;

        if (!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack && (_oldIsTouchingWall && _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();

        StartCoyoteTime();
    }

    public override void Exit()
    {
        base.Exit();

        _oldIsTouchingWall = false;
        _oldIsTouchingWallBack = false;
    }

    public override void DoActions()
    {
        base.DoActions();

        CheckJumpMultiplier();
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        movement?.CheckMovementDirection(_inputX);
        movement?.SetVelocityX(_playerData.movementVelocity * _inputX * _airControlPercentage);

        visualController?.SetAnimationFloat("velocityX", Mathf.Abs(movement._currentVelocity.x));
        visualController?.SetAnimationFloat("velocityY", movement._currentVelocity.y);
    }
    
    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_isPressingPrimaryAttack)
        {
            //stateMachine?.ChangeState(player.primaryAttackState);
        }
        else if (_isPressingSecondaryAttack)
        {
            stateMachine?.ChangeState(_player.secondaryAttackState);
        }
        else if (_isPressingJump && inputHandler.CanJump())
        {
            if (_isPressingCrouch)
            {
                stateMachine?.ChangeState(_player.crouchJumpState);
            }
            else if (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime)
            {
                stateMachine?.ChangeState(_player.wallJumpState);
            }
            else
            {
                stateMachine?.ChangeState(_player.jumpState);
            }
        }
        else if (_isGrounded && movement._currentVelocity.y < 0.01)
        {
            if (_isPressingCrouch)
            {
                stateMachine?.ChangeState(_player.crouchLandState);
            }
            else
            {
                stateMachine?.ChangeState(_player.landState);
            }
        }
        else if (_isTouchingWall && !_isPressingCrouch)
        {
            if (_isPressingGrab && _isTouchingLedge)
            {
                stateMachine?.ChangeState(_player.wallGrabState);
            }
            else if (_inputX == movement._movementDir && movement._currentVelocity.y <= 0f)
            {
                stateMachine?.ChangeState(_player.wallSlideState);
            }
            else if (!_isTouchingLedge && !_isGrounded && !_isTouchingCeiling)
            {
                stateMachine?.ChangeState(_player.ledgeClimbState);
            }
        }
        else if (_isPressingCrouch && inputHandler.CanCrouch() && !_isGrounded)
        {
            stateMachine?.ChangeState(_player.crouchInAirState);
        }
    }

    private void CheckCoyoteTime()
    {
        if (_coyoteTime && Time.time > _startTime + _playerData.coyoteTime)
        {
            _coyoteTime = false;
            inputHandler.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (_wallJumpCoyoteTime && Time.time > _wallJumpCoyoteTimeStart + _playerData.coyoteTime)
        {
            _wallJumpCoyoteTime = false;
            _wallJumpCoyoteTimeStart = Time.time;
        }
    }

    public void StartWallJumpCoyoteTime()
    {
        _wallJumpCoyoteTime = true;
        _wallJumpCoyoteTimeStart = Time.time;
    }

    private void CheckJumpMultiplier()
    {
        if (!_isJumping)
        {
            return;
        }

        if (_isJumpCanceled)
        {
            movement?.SetVelocityY(movement._currentVelocity.y * _playerData.variableJumpHeightMultiplier);
            _isJumping = false;
        }
        else if (movement._currentVelocity.y <= 0f)
        {
            _isJumping = false;
        }
    }

    public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;
    public void StartCoyoteTime() => _coyoteTime = true;
    public void SetIsJumping() => _isJumping = true;
}