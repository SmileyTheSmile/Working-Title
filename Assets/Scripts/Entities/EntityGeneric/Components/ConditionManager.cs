using UnityEngine;

//TODO Get rid of this class and move everything in proper components
public class ConditionManager : CoreComponent
{
    protected Movement movement
    { get => _movement ?? _entity.GetCoreComponent(ref _movement); }
    private Movement _movement;

    [SerializeField] private PlayerData _playerData;

    public CollisionCheckTransitionCondition IsGroundedSO;
    public InputTransitionCondition IsMovingXSO;
    public InputTransitionCondition IsMovingUpSO;
    public InputTransitionCondition IsMovingDownSO;
    public SupportTransitionCondition HasStoppedFalling;
    public SupportTransitionCondition CanCrouchSO;
    public SupportTransitionCondition CanJumpSO;
    public SupportTransitionCondition IsMovingInCorrectDirSO;

    public ScriptableInt _normalizedInputXSO;
    public ScriptableInt _normalizedInputYSO;
    public ScriptableInt _movementDirSO;

    public AudioSourcePlayer fallSound;
    public AudioSourcePlayer moveSound;
    public AudioSourcePlayer jumpSound;
    public float stepDelay;

    private int _amountOfJumpsLeft;
    private int _amountOfCrouchesLeft;

    private void Awake()
    {
        //TODO Fix this not working after ScriptableObject references are made on second compile
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