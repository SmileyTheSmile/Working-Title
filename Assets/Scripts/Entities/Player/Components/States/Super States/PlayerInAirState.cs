using UnityEngine;

[CreateAssetMenu(fileName = "Player In Air State", menuName = "States/Player/In Air/In Air State")]

public class PlayerInAirState : PlayerState
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    protected int _inputX;
    protected int _inputY;
    protected bool _dashInput;
    protected bool _grabInput;
    protected bool _crouchInput;
    protected bool _jumpInput;
    protected bool _jumpInputStop;
    protected Vector2 _mousePositionInput;

    protected bool _isGrounded;
    protected bool _isTouchingWall;
    protected bool _oldIsTouchingWall;
    protected bool _isTouchingWallBack;
    protected bool _oldIsTouchingWallBack;
    protected bool _isTouchingLedge;
    protected bool _isTouchingCeiling;
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

        _isGrounded = collisionSenses.Ground;
        _isTouchingWall = collisionSenses.WallFront;
        _isTouchingWallBack = collisionSenses.WallBack;
        _isTouchingLedge = collisionSenses.LedgeHorizontal;

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
        _isTouchingWall = false;
        _isTouchingWallBack = false;
    }

    public override void DoActions()
    {
        base.DoActions();

        _inputX = inputHandler.normalizedInputX;
        _inputY = inputHandler.normalizedInputY;
        _grabInput = inputHandler.grabInput;
        _crouchInput = inputHandler.crouchInput;
        _jumpInput = inputHandler.jumpInput;
        _jumpInputStop = inputHandler.jumpInputStop;
        _mousePositionInput = inputHandler.mousePositionInput;

        CheckJumpMultiplier();
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        movement?.CheckMovementDirection(_inputX);
    }
    
    public override void DoTransitions()
    {
        base.DoTransitions();

        if (inputHandler._attackInputs[(int)CombatInputs.primary])
        {
            //stateMachine?.ChangeState(player.primaryAttackState);
        }
        else if (inputHandler._attackInputs[(int)CombatInputs.secondary])
        {
            stateMachine?.ChangeState(_player.secondaryAttackState);
        }
        else if (_jumpInput && inputHandler.CanJump())
        {
            if (_crouchInput)
            {
                stateMachine?.ChangeState(_player.crouchJumpState);
            }
            else if (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime)
            {
                StopWallJumpCoyoteTime();
                _isTouchingWall = collisionSenses.WallFront;
                _player.wallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                stateMachine?.ChangeState(_player.wallJumpState);
            }
            else
            {
                stateMachine?.ChangeState(_player.jumpState);
            }
        }

        if (_isGrounded && movement._currentVelocity.y < 0.01)
        {
            if (_crouchInput)
            {
                stateMachine?.ChangeState(_player.crouchLandState);
            }
            else
            {
                stateMachine?.ChangeState(_player.landState);
            }
        }
        else if (_isTouchingWall && !_crouchInput)
        {
            if (_grabInput && _isTouchingLedge)
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
        else if (_crouchInput && _player.crouchInAirState.CanCrouch() && !_isGrounded)
        {
            stateMachine?.ChangeState(_player.crouchInAirState);
        }
        else
        {
            movement?.SetVelocityX(_playerData.movementVelocity * _inputX * _airControlPercentage);

            visualController?.SetAnimationFloat("velocityX", Mathf.Abs(movement._currentVelocity.x));
            visualController?.SetAnimationFloat("velocityY", movement._currentVelocity.y);
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

        if (_jumpInputStop)
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