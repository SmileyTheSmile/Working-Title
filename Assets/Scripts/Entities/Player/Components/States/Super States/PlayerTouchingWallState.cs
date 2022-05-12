using UnityEngine;

public abstract class PlayerTouchingWallState : PlayerState
{
    private ConditionManager conditionManager
    { get => _conditionManager ?? core.GetCoreComponent(ref _conditionManager); }
    private ConditionManager _conditionManager;

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected int _inputX => inputHandler.normalizedInputX;
    protected int _inputY => inputHandler.normalizedInputY;

    protected bool _isPressingGrab => conditionManager.IsPressingGrabSO.value;
    protected bool _isPressingJump => conditionManager.IsPressingJumpSO.value;
    protected bool _isPressingCrouch => conditionManager.IsPressingCrouchSO.value;

    protected bool _isGrounded => conditionManager.IsGroundedSO.value;
    protected bool _isTouchingWall => conditionManager.IsTouchingWallFrontSO.value;
    protected bool _isTouchingLedge => conditionManager.IsTouchingLedgeHorizontalSO.value;
    protected bool _isTouchingCeiling => conditionManager.IsTouchingCeilingSO.value;

    public PlayerTouchingWallState(Player player, string animBoolName, PlayerData playerData)
    : base(player, animBoolName, playerData) { }
    
    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_isPressingJump)
        {
            stateMachine?.ChangeState(_player.wallJumpState);
        }
        else if (_isGrounded && !_isPressingGrab)
        {
            stateMachine?.ChangeState(_player.idleState);
        }
        else if (_isGrounded && _isPressingCrouch && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.crouchIdleState);
        }
        else if (!_isTouchingWall || (_inputX != movement._movementDir && !_isPressingGrab))
        {
            stateMachine?.ChangeState(_player.inAirState);
        }
        else if (_isTouchingWall && !_isTouchingLedge && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.ledgeClimbState);
        }
    }
}
