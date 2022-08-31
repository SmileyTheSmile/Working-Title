using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
    protected Movement _movement;

    [SerializeField] private PlayerConditionTable _playerConditionTable;
    [SerializeField] protected PlayerAttackState primaryAttackState;
    [SerializeField] protected PlayerAttackState secondaryAttackState;
    [SerializeField] protected PlayerJumpState jumpState;
    [SerializeField] protected PlayerCrouchJumpState crouchJumpState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] protected PlayerCrouchInAirState crouchInAirState;
    [SerializeField] protected PlayerWallGrabState wallGrabState;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingCeilingSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingWallFrontSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingLedgeHorizontalSO;
    
    protected int _inputX => _playerConditionTable.NormalizedInputX;

    protected bool _isPressingGrab => _playerConditionTable.IsPressingGrab;
    protected bool _isPressingCrouch => _playerConditionTable.IsPressingCrouch;
    protected bool _isPressingJump => _playerConditionTable.IsPressingJump;
    protected bool _isPressingPrimaryAttack => _playerConditionTable.IsPressingPrimaryAttack;
    protected bool _isPressingSecondaryAttack => _playerConditionTable.IsPressingSecondaryAttack;
    protected bool _isMovingX => _playerConditionTable.IsMovingX;

    protected bool _isGrounded => _playerConditionTable.IsGrounded;
    protected bool _isTouchingCeiling => IsTouchingCeilingSO.value;
    protected bool _isTouchingWall => IsTouchingWallFrontSO.value;
    protected bool _isTouchingLedge => IsTouchingLedgeHorizontalSO.value;

    protected bool _canCrouch => _playerConditionTable.CanCrouch;
    protected bool _canJump => _playerConditionTable.CanJump;
    protected bool _isJumping => _playerConditionTable.IsPressingJump;
    protected bool _canAttack => _playerConditionTable.CanAttack;
    protected bool _isReloading => _playerConditionTable.IsReloading;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _movement = _core.GetCoreComponent<Movement>();
    }

    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.ResetAmountOfJumpsLeft();
        _temporaryComponent.ResetAmountOfCrouchesLeft();
    }

    public override void DoActions()
    {
        base.DoActions();

        _temporaryComponent.CheckMovementDirection(_inputX);
    }

    public override GenericState DoTransitions()
    {
        if (_isPressingPrimaryAttack && !_isTouchingCeiling && _canAttack && !_isReloading)
        {
            return primaryAttackState;
        }
        else if (_isPressingSecondaryAttack && !_isTouchingCeiling)
        {
            //return secondaryAttackState;
            return null;
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