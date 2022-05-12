using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
    private ConditionManager conditionManager
    { get => _conditionManager ?? core.GetCoreComponent(ref _conditionManager); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private ConditionManager _conditionManager;
    private Movement _movement;

    protected int _inputX => inputHandler.normalizedInputX;
    protected int _inputY => inputHandler.normalizedInputY;
    protected Vector2 _mousePositionInput => inputHandler.mousePositionInput;

    protected bool _isPressingGrab => conditionManager.IsPressingGrabSO.value;
    protected bool _isPressingCrouch => conditionManager.IsPressingCrouchSO.value;
    protected bool _isPressingJump => conditionManager.IsPressingJumpSO.value;
    protected bool _isPressingPrimaryAttack => conditionManager.IsPressingPrimaryAttackSO.value;
    protected bool _isPressingSecondaryAttack => conditionManager.IsPressingSecondaryAttackSO.value;

    protected bool _isGrounded => conditionManager.IsGroundedSO.value;
    protected bool _isTouchingCeiling => conditionManager.IsTouchingCeilingSO.value;
    protected bool _isTouchingWall => conditionManager.IsTouchingWallFrontSO.value;
    protected bool _isTouchingLedge => conditionManager.IsTouchingLedgeHorizontalSO.value;

    public PlayerGroundedState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        inputHandler?.ResetAmountOfJumpsLeft();

        inputHandler?.ResetAmountOfCrouchesLeft();
    }

    public override void DoActions()
    {
        base.DoActions();

        movement?.CheckMovementDirection(_inputX);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();
        
        if (_isPressingPrimaryAttack && !_isTouchingCeiling)
        {
            //stateMachine?.ChangeState(player.primaryAttackState);
        }
        else if (_isPressingSecondaryAttack && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.secondaryAttackState);
        }
        else if ((_isPressingJump && inputHandler.CanJump() && !_isTouchingCeiling))
        {
            if (_isPressingCrouch && inputHandler.CanCrouch())
            {
                stateMachine?.ChangeState(_player.crouchJumpState);
            }
            else
            {
                stateMachine?.ChangeState(_player.jumpState);
            }
        }
        else if (!_isGrounded)
        {
            if (_isPressingCrouch)
            {
                stateMachine?.ChangeState(_player.crouchInAirState);
            }
            else
            {
                stateMachine?.ChangeState(_player.inAirState);
            }
        }
        else if (_isTouchingWall && _isPressingGrab && _isTouchingLedge && !_isTouchingCeiling && !_isPressingCrouch)
        {
            stateMachine?.ChangeState(_player.wallGrabState);
        }
    }
}