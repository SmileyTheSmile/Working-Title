using UnityEngine;

public class ConditionManager : CoreComponent
{
    protected Movement movement
    { get => _movement ?? _entity.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected PlayerInputHandler inputHandler
    { get => _inputHandler ?? _entity.GetCoreComponent(ref _inputHandler); }
    private PlayerInputHandler _inputHandler;

    public CollisionCheckTransitionCondition IsGroundedSO;
    public CollisionCheckTransitionCondition IsTouchingCeilingSO;
    public CollisionCheckTransitionCondition IsTouchingWallFrontSO;
    public CollisionCheckTransitionCondition IsTouchingWallBackSO;
    public CollisionCheckTransitionCondition IsTouchingLedgeHorizontalSO;
    public CollisionCheckTransitionCondition IsTouchingLedgeVerticalSO;

    public InputTransitionCondition IsPressingJumpSO;
    public InputTransitionCondition IsJumpCanceledSO;
    public InputTransitionCondition IsPressingGrabSO;
    public InputTransitionCondition IsPressingCrouchSO;
    public InputTransitionCondition IsPressingPrimaryAttackSO;
    public InputTransitionCondition IsPressingSecondaryAttackSO;
    public InputTransitionCondition IsMovingXSO;
    public InputTransitionCondition IsMovingUpSO;
    public InputTransitionCondition IsMovingDownSO;

    public SupportTransitionCondition IsJumpingSO;
    public SupportTransitionCondition HasStoppedFalling;
    public SupportTransitionCondition CanCrouchSO;
    public SupportTransitionCondition CanJumpSO;
    public SupportTransitionCondition IsMovingInCorrectDirSO;

    public ScriptableInt _normalizedInputXSO;
    public ScriptableInt _normalizedInputYSO;
    public ScriptableInt _movementDirSO;

    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerJumpState jumpState;
    public PlayerInAirState inAirState;
    public PlayerLandState landState;

    public PlayerWallSlideState wallSlideState;
    public PlayerWallGrabState wallGrabState;
    public PlayerWallClimbState wallClimbState;
    public PlayerWallJumpState wallJumpState;
    public PlayerLedgeClimbState ledgeClimbState;

    public PlayerCrouchIdleState crouchIdleState;
    public PlayerCrouchMoveState crouchMoveState;
    public PlayerCrouchJumpState crouchJumpState;
    public PlayerCrouchInAirState crouchInAirState;
    public PlayerCrouchLandState crouchLandState;

    public PlayerAttackState primaryAttackState;
    public PlayerAttackState secondaryAttackState;
    
    [SerializeField] private PlayerData _playerData;

    private int _amountOfJumpsLeft;
    private int _amountOfCrouchesLeft;

    private void Awake()
    {
        ResetAmountOfJumpsLeft();
        ResetAmountOfCrouchesLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HasStoppedFalling.value = movement.currentVelocity.y < 0.01;
        CanCrouchSO.value = CanCrouch();
        CanJumpSO.value = CanJump();
        IsMovingInCorrectDirSO.value = (_normalizedInputXSO.value == _movementDirSO.value);

        switch (_normalizedInputXSO.value)
        {
            case 0:
                IsMovingXSO.value = false;
                break;
            default:
                IsMovingXSO.value = true;
                break;
        }

        switch (_normalizedInputYSO.value)
        {
            case > 0:
                IsMovingUpSO.value = true;
                IsMovingDownSO.value = false;
                break;
            case < 0:
                IsMovingUpSO.value = false;
                IsMovingDownSO.value = true;
                break;
            default:
                IsMovingUpSO.value = false;
                IsMovingDownSO.value = false;
                break;
        }
    }

    public bool CanCrouch() => (_amountOfCrouchesLeft > 0);
    public void ResetAmountOfCrouchesLeft() => _amountOfCrouchesLeft = _playerData.amountOfCrouches;
    public void DecreaseAmountOfCrouchesLeft() => _amountOfCrouchesLeft--;

    public bool CanJump() => (_amountOfJumpsLeft > 0);
    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.amountOfJumps;
    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
}