using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
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
    protected bool _isCrouching;
    protected bool _isTouchingCeiling;
    protected bool _isTouchingWall;
    protected bool _isTouchingLedge;

    public PlayerGroundedState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        if (collisionSenses)
        {
            _isGrounded = collisionSenses.Ground;
            _isTouchingCeiling = collisionSenses.Ceiling;
            _isTouchingWall = collisionSenses.WallFront;
            _isTouchingLedge = collisionSenses.LedgeHorizontal;
        }
    }

    public override void Enter()
    {
        base.Enter();

        _player.jumpState.ResetAmountOfJumpsLeft();
        _player.crouchInAirState.ResetAmountOfCrouchesLeft();
        _player.dashState.ResetCanDash();
    }

    public override void DoActions()
    {
        base.DoActions();

        _inputX = inputHandler.normalizedInputX;
        _inputY = inputHandler.normalizedInputY;
        _grabInput = inputHandler.grabInput;
        _dashInput = inputHandler.dashInput;
        _jumpInput = inputHandler.jumpInput;
        _jumpInputStop = inputHandler.jumpInputStop;
        _crouchInput = inputHandler.crouchInput;
        _mousePositionInput = inputHandler.mousePositionInput;

        movement?.CheckMovementDirection(_inputX);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();
        
        //Ability States
        if (inputHandler.attackInputs[(int)CombatInputs.primary] && !_isTouchingCeiling)
        {
            //stateMachine?.ChangeState(player.primaryAttackState);
        }
        else if (inputHandler.attackInputs[(int)CombatInputs.secondary] && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.secondaryAttackState);
        }
        else if ((_jumpInput && _player.jumpState.CanJump() && !_isTouchingCeiling))
        {
            if (_crouchInput && _player.crouchInAirState.CanCrouch())
            {
                stateMachine?.ChangeState(_player.crouchJumpState);
            }
            else
            {
                stateMachine?.ChangeState(_player.jumpState);
            }
        }
        else if (_dashInput && _player.dashState.CheckIfCanDash() && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.dashState);
        }

        //Other States
        if (!_isGrounded)
        {
            if (_crouchInput)
            {
                stateMachine?.ChangeState(_player.crouchInAirState);
            }
            else
            {
                stateMachine?.ChangeState(_player.inAirState);
            }
        }
        else if (_isTouchingWall && _grabInput && _isTouchingLedge && !_isTouchingCeiling && !_crouchInput)
        {
            stateMachine?.ChangeState(_player.wallGrabState);
        }
    }
}