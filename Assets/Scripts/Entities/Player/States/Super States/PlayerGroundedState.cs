using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
    protected Movement movement
    { get => _movement ?? _entity.GetCoreComponent(ref _movement); }
    private Movement _movement;

    [SerializeField] protected PlayerAttackState primaryAttackState;
    [SerializeField] protected PlayerAttackState secondaryAttackState;
    [SerializeField] protected PlayerJumpState jumpState;
    [SerializeField] protected PlayerCrouchJumpState crouchJumpState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] protected PlayerCrouchInAirState crouchInAirState;
    [SerializeField] protected PlayerWallGrabState wallGrabState;

    [SerializeField] protected ScriptableInt InputXSO;

    [SerializeField] protected InputTransitionCondition IsPressingGrabSO;
    [SerializeField] protected InputTransitionCondition IsPressingCrouchSO;
    [SerializeField] protected InputTransitionCondition IsPressingJumpSO;
    [SerializeField] protected InputTransitionCondition IsPressingPrimaryAttackSO;
    [SerializeField] protected InputTransitionCondition IsPressingSecondaryAttackSO;
    [SerializeField] protected InputTransitionCondition IsMovingXSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsGroundedSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingCeilingSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingWallFrontSO;
    [SerializeField] protected CollisionCheckTransitionCondition IsTouchingLedgeHorizontalSO;
    [SerializeField] protected SupportTransitionCondition CanCrouchSO;
    [SerializeField] protected SupportTransitionCondition CanJumpSO;
    [SerializeField] protected SupportTransitionCondition IsJumpingSO;
    [SerializeField] protected SupportTransitionCondition CanAttackSO;
    [SerializeField] protected SupportTransitionCondition IsReloadingSO;

    protected int _inputX => InputXSO.value;

    protected bool _isPressingGrab => IsPressingGrabSO.value;
    protected bool _isPressingCrouch => IsPressingCrouchSO.value;
    protected bool _isPressingJump => IsPressingJumpSO.value;
    protected bool _isPressingPrimaryAttack => IsPressingPrimaryAttackSO.value;
    protected bool _isPressingSecondaryAttack => IsPressingSecondaryAttackSO.value;
    protected bool _isMovingX => IsMovingXSO.value;

    protected bool _isGrounded => IsGroundedSO.value;
    protected bool _isTouchingCeiling => IsTouchingCeilingSO.value;
    protected bool _isTouchingWall => IsTouchingWallFrontSO.value;
    protected bool _isTouchingLedge => IsTouchingLedgeHorizontalSO.value;

    protected bool _canCrouch => CanCrouchSO.value;
    protected bool _canJump => CanJumpSO.value;
    protected bool _isJumping => IsJumpingSO.value;
    protected bool _canAttack => CanAttackSO.value;
    protected bool _isReloading => IsReloadingSO.value;

    public override void Enter()
    {
        base.Enter();

        conditionManager.ResetAmountOfJumpsLeft();
        conditionManager.ResetAmountOfCrouchesLeft();
    }

    public override void DoActions()
    {
        base.DoActions();

        movement.CheckMovementDirection(_inputX);
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