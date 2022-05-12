using UnityEngine;

public abstract class PlayerTouchingWallState : PlayerState
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    protected int _inputX;
    protected int _inputY;
    protected bool _grabInput;
    protected bool _jumpInput;
    protected bool _crouchInput;

    protected bool _isGrounded => collisionSenses._groundCheck.value;
    protected bool _isTouchingWall => collisionSenses._wallFrontCheck.value;
    protected bool _isTouchingLedge => collisionSenses._ledgeHorizontalCheck.value;
    protected bool _isTouchingCeiling => collisionSenses._ceilingCheck.value;

    public PlayerTouchingWallState(Player player, string animBoolName, PlayerData playerData)
    : base(player, animBoolName, playerData) { }

    public override void DoActions()
    {
        base.DoActions();

        _inputX = inputHandler.normalizedInputX;
        _inputY = inputHandler.normalizedInputY;
        _grabInput = inputHandler.grabInput;
        _jumpInput = inputHandler.jumpInput;
        _crouchInput = inputHandler.crouchInput;
    }
    
    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_jumpInput)
        {
            _player.wallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            stateMachine?.ChangeState(_player.wallJumpState);
        }
        else if (_isGrounded && !_grabInput)
        {
            stateMachine?.ChangeState(_player.idleState);
        }
        else if (_isGrounded && _crouchInput && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.crouchIdleState);
        }
        else if (!_isTouchingWall || (_inputX != movement._movementDir && !_grabInput))
        {
            stateMachine?.ChangeState(_player.inAirState);
        }
        else if (_isTouchingWall && !_isTouchingLedge && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.ledgeClimbState);
        }
    }
}
