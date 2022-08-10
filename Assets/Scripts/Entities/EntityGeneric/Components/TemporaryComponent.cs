using UnityEngine;
using System;

//TODO Get rid of this class and move everything in proper components
public class TemporaryComponent : CoreComponent
{
    protected Movement movement
    { get => _movement ?? _entity.GetCoreComponent(ref _movement); }
    private Movement _movement;
    private WeaponHandler weaponHandler
    { get => _weaponHandler ?? _entity.GetCoreComponent(ref _weaponHandler); }
    private WeaponHandler _weaponHandler;

    private VisualController visualController
    { get => _visualController ?? _entity.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _ceilingCheckTransform;
    [SerializeField] private CollisionCheckTransitionCondition _ceilingCheck;
    [SerializeField] private CollisionCheckTransitionCondition _wallFront;
    
    public ScriptableInt _movementDirSO;
    public CollisionCheckTransitionCondition IsGroundedSO;
    public InputTransitionCondition IsMovingXSO;
    public InputTransitionCondition IsMovingUpSO;
    public InputTransitionCondition IsMovingDownSO;
    public InputTransitionCondition _isPressingJumpSO;
    public InputTransitionCondition _isJumpCanceledSO;
    public InputTransitionCondition _isPressingGrabSO;
    public InputTransitionCondition _isCrouchingSO;
    public InputTransitionCondition _isPressingPrimaryAttackSO;
    public InputTransitionCondition _isPressingSecondaryAttackSO;
    public InputTransitionCondition _isPressingPauseSO;
    public SupportTransitionCondition HasStoppedFalling;
    public SupportTransitionCondition CanCrouchSO;
    public SupportTransitionCondition CanJumpSO;
    public SupportTransitionCondition IsMovingInCorrectDirSO;

    public ScriptableInt _normalizedInputXSO;
    public ScriptableInt _normalizedInputYSO;
    public ScriptableInt _weaponSwitchInputSO;

    public AudioSourcePlayer fallSound;
    public AudioSourcePlayer moveSound;
    public AudioSourcePlayer jumpSound;

    public ScriptableVector3 _mousePositionInputSO;
    public PlayerInputHandler _playerInputHandler;
    private float _jumpInputStartTime;

    private Camera _mainCamera;
    public float stepDelay;
    private int _facingDir;

    private Vector2 _rawMouseInput;
    private int _amountOfJumpsLeft;
    private int _amountOfCrouchesLeft;
    private PlayerCrouchingForm _crouchingForm;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _crouchingForm = PlayerCrouchingForm.notCrouching;
        _facingDir = 1;
        _movementDirSO.value = 1;

        _jumpInputStartTime = Time.time;//jump input
        //TODO Fix this not working after ScriptableObject references are made on second compile
        ResetAmountOfJumpsLeft();
        ResetAmountOfCrouchesLeft();
    }

    private void OnEnable()
    {
        _playerInputHandler.AimEvent += OnAim;
        _playerInputHandler.JumpEvent += OnJump;
        _playerInputHandler.GrabEvent += OnGrab;
        _playerInputHandler.CrouchEvent += OnCrouch;
        _playerInputHandler.MoveEvent += OnMovement;
        _playerInputHandler.WeaponSwitchEvent += OnWeaponSwitch;
        _playerInputHandler.JumpCanceledEvent += OnJumpCancelled;
        _playerInputHandler.PrimaryAttackEvent += OnPrimaryAttack;
        _playerInputHandler.SecondaryAttackEvent += OnSecondaryAttack;
    }

    private void OnDisable()
    {
        _playerInputHandler.AimEvent -= OnAim;
        _playerInputHandler.JumpEvent -= OnJump;
        _playerInputHandler.GrabEvent -= OnGrab;
        _playerInputHandler.CrouchEvent -= OnCrouch;
        _playerInputHandler.MoveEvent -= OnMovement;
        _playerInputHandler.WeaponSwitchEvent -= OnWeaponSwitch;
        _playerInputHandler.JumpCanceledEvent -= OnJumpCancelled;
        _playerInputHandler.PrimaryAttackEvent -= OnPrimaryAttack;
        _playerInputHandler.SecondaryAttackEvent -= OnSecondaryAttack;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HasStoppedFalling.value = movement.CurrentVelocity.y < 0.01;
        CanCrouchSO.value = CanCrouch();
        CanJumpSO.value = CanJump();
        IsMovingInCorrectDirSO.value = (_normalizedInputXSO.value == _movementDirSO.value);

        if (_normalizedInputXSO.value == 0)
            IsMovingXSO.value = false;
        else
            IsMovingXSO.value = true;

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

        ProcessMouseInput();
        CheckJumpInputHoldTime();
    }

    public void OnMovement(Vector2 value)
    {
        _normalizedInputXSO.value = Mathf.RoundToInt(value.x);
        _normalizedInputYSO.value = Mathf.RoundToInt(value.y);
    }

    public void OnJump()
    {
        _isPressingJumpSO.value = true;
        _isJumpCanceledSO.value = false;

        _jumpInputStartTime = Time.time;
    }

    public void OnJumpCancelled()
    {
        _isPressingJumpSO.value = false;
        _isJumpCanceledSO.value = true;
    }

    public void OnGrab(bool value)
    {
        _isPressingGrabSO.value = value;
    }

    public void OnPrimaryAttack(bool value)
    {
        _isPressingPrimaryAttackSO.value = value;
    }

    public void OnSecondaryAttack(bool value)
    {
        _isPressingSecondaryAttackSO.value = value;
    }

    public void OnCrouch(bool value)
    {
        _isCrouchingSO.value = value;
    }

    public void OnAim(Vector2 value)
    {
        _rawMouseInput = value;
    }

    public void OnWeaponSwitch(Vector2 value)
    {
        _weaponSwitchInputSO.value = (int)value.y;
    }

    public void OnPause()
    {

    }

    private void ProcessMouseInput()
    {
        Vector3 shiftedMouseInput = new Vector3(_rawMouseInput.x, _rawMouseInput.y, 10);

        _mousePositionInputSO.value = _mainCamera.ScreenToWorldPoint(shiftedMouseInput);
    }


    //Check if jump button has been held for the value in inputHoldTime
    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= _jumpInputStartTime + _playerData.jumpInputHoldTime)
        {
            _isPressingJumpSO.value = false;
        }
    }

    public void CrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (_crouchingForm == PlayerCrouchingForm.notCrouching && crouchInput)
        {
            _crouchingForm = PlayerCrouchingForm.crouchingDown;

            SquashColliderDown(biggerHeight, smallerHeight);
        }
    }

    public void UnCrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (((_crouchingForm == PlayerCrouchingForm.crouchingDown && !crouchInput)
        || !_wallFront.value) && !_ceilingCheck.value)
        {
            _crouchingForm = PlayerCrouchingForm.notCrouching;

            ResetColliderHeight(biggerHeight, smallerHeight);
        }
    }

    public void MoveCeilingCheck(float oldHeight, float newHeight, float defaultColliderHeight)
    {
        _ceilingCheckTransform.position += Vector3.up * ((oldHeight - newHeight) * defaultColliderHeight);
    }

    public void ResetColliderHeight(float biggerHeight, float smallerHeight)
    {
        movement.SetColliderSize(movement.DefaultSize);
        movement.SetColliderOffset(Vector2.zero);

        MoveCeilingCheck(biggerHeight, smallerHeight, movement.DefaultSize.y);
    }

    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        float height = movement.ColliderSize.y * smallerHeight;

        movement.SetColliderOffsetY((height - movement.ColliderSize.y) / 2);
        movement.SetColliderSizeY(height);

        MoveCeilingCheck(smallerHeight, biggerHeight, movement.DefaultSize.y);
    }

    public void Flip()
    {
        _facingDir *= -1;

        weaponHandler?.FlipWeapon(_facingDir);
        visualController?.FlipEntity(_facingDir);
    }

    //Change the movement direction of the entity based on the x input
    public void CheckMovementDirection(int inputX)
    {
        if (inputX != 0 && inputX != _movementDirSO.value)
        {
            _movementDirSO.value *= -1;
        }
    }

    //Change the facing direction of the entity based on the mouse position
    public void CheckFacingDirection(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 mouseDirection = (mousePos - playerPos).normalized;

        float angle = Vector2.SignedAngle(Vector2.right, mouseDirection);
        angle = (angle > 90) ? angle - 270 : angle + 90;

        if (Math.Sign(angle) != _facingDir)
        {
            Flip();
        }
    }

    public void UseJumpInput() => _isPressingJumpSO.value = false;
    public bool CanCrouch() => (_amountOfCrouchesLeft > 0);
    public void ResetAmountOfCrouchesLeft() => _amountOfCrouchesLeft = _playerData.amountOfCrouches;
    public void DecreaseAmountOfCrouchesLeft() => _amountOfCrouchesLeft--;
    public bool CanJump() => (_amountOfJumpsLeft > 0);
    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.amountOfJumps;
    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;

    private enum PlayerCrouchingForm
    {
        notCrouching,
        crouchingDown,
        crouchingUp
    }
}