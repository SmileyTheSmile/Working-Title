using UnityEngine;
using System;

//TODO Get rid of this class and move everything in proper components
public class TemporaryComponent : CoreComponent
{
    private Movement _movement;
    private WeaponHandler _weaponHandler;
    private VisualController _visualController;
    [SerializeField] private PlayerInputHandler _playerInputHandler;

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerConditionTable _conditions;

    [SerializeField] private Transform _ceilingCheckTransform;
    [SerializeField] private CollisionCheckTransitionCondition _ceilingCheck;
    [SerializeField] private CollisionCheckTransitionCondition _wallFront;

    private float _jumpInputStartTime;

    private Camera _mainCamera;
    public float stepDelay;
    private int _facingDir;

    private Vector2 _rawMouseInput;
    private int _amountOfJumpsLeft;
    private int _amountOfCrouchesLeft;
    private PlayerCrouchingForm _crouchingForm;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);
        
        _mainCamera = Camera.main;
        _crouchingForm = PlayerCrouchingForm.notCrouching;
        _facingDir = 1;
        _conditions.MovementDir = 1;
        _jumpInputStartTime = Time.time;//jump input
        
        //TODO Fix this not working after ScriptableObject references are made on second compile
        ResetAmountOfJumpsLeft();
        ResetAmountOfCrouchesLeft();
    }

    public override void SetupConnections()
    {
        base.SetupConnections();

        _movement = _core.GetCoreComponent<Movement>();
        _weaponHandler = _core.GetCoreComponent<WeaponHandler>();
        _visualController = _core.GetCoreComponent<VisualController>();
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

        _conditions.HasStoppedFalling = _movement.CurrentVelocity.y < 0.01;
        _conditions.CanCrouch = CanCrouch();
        _conditions.CanJump = CanJump();
        _conditions.IsMovingInCorrectDir = (_conditions.NormalizedInputX == _conditions.MovementDir);

        if (_conditions.NormalizedInputX == 0)
            _conditions.IsMovingX = false;
        else
            _conditions.IsMovingX = true;

        switch (_conditions.NormalizedInputY)
        {
            case > 0:
                _conditions.IsMovingUp = true;
                _conditions.IsMovingDown = false;
                break;
            case < 0:
                _conditions.IsMovingUp = false;
                _conditions.IsMovingDown = true;
                break;
            default:
                _conditions.IsMovingUp = false;
                _conditions.IsMovingDown = false;
                break;
        }
        ProcessMouseInput();
        CheckJumpInputHoldTime();
    }

    public void OnMovement(Vector2 value)
    {
        _conditions.NormalizedInputX = Mathf.RoundToInt(value.x);
        _conditions.NormalizedInputY = Mathf.RoundToInt(value.y);
    }

    public void OnJump()
    {
        _conditions.IsPressingJump = true;
        _conditions.IsJumpCanceled = false;

        _jumpInputStartTime = Time.time;
    }

    public void OnJumpCancelled()
    {
        _conditions.IsPressingJump = false;
        _conditions.IsJumpCanceled = true;
    }

    public void OnGrab(bool value)
    {
        _conditions.IsPressingGrab = value;
    }

    public void OnPrimaryAttack(bool value)
    {
        _conditions.IsPressingPrimaryAttack = value;
    }

    public void OnSecondaryAttack(bool value)
    {
        _conditions.IsPressingSecondaryAttack = value;
    }

    public void OnCrouch(bool value)
    {
        _conditions.IsPressingCrouch = value;
    }

    public void OnAim(Vector2 value)
    {
        _rawMouseInput = value;
    }

    public void OnWeaponSwitch(Vector2 value)
    {
        _conditions.WeaponSwitchInput = (int)value.y;
    }

    public void OnPause()
    {

    }

    private void ProcessMouseInput()
    {
        Vector3 shiftedMouseInput = new Vector3(_rawMouseInput.x, _rawMouseInput.y, 10);

        _conditions.MousePosition = _mainCamera.ScreenToWorldPoint(shiftedMouseInput);
    }


    //Check if jump button has been held for the value in inputHoldTime
    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= _jumpInputStartTime + _playerData.jumpInputHoldTime)
        {
            _conditions.IsPressingJump = false;
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
        _movement.SetColliderSize(_movement.DefaultSize);
        _movement.SetColliderOffset(Vector2.zero);

        MoveCeilingCheck(biggerHeight, smallerHeight, _movement.DefaultSize.y);
    }

    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        float height = _movement.ColliderSize.y * smallerHeight;

        _movement.SetColliderOffsetY((height - _movement.ColliderSize.y) / 2);
        _movement.SetColliderSizeY(height);

        MoveCeilingCheck(smallerHeight, biggerHeight, _movement.DefaultSize.y);
    }

    public void Flip()
    {
        _facingDir *= -1;

        _weaponHandler?.FlipWeapon(_facingDir);
        _visualController?.FlipEntity(_facingDir);
    }

    //Change the movement direction of the entity based on the x input
    public void CheckMovementDirection(int inputX)
    {
        if (inputX != 0 && inputX != _conditions.MovementDir)
        {
            _conditions.MovementDir *= -1;
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

    public void UseJumpInput() => _conditions.IsPressingJump = false;
    public bool CanCrouch() => (_amountOfCrouchesLeft > 0);
    public void ResetAmountOfCrouchesLeft() => _amountOfCrouchesLeft = _playerData.amountOfCrouches;
    public void DecreaseAmountOfCrouchesLeft() => _amountOfCrouchesLeft--;
    public bool CanJump() => (_amountOfJumpsLeft > 0);
    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.amountOfJumps;
    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
}

public enum PlayerCrouchingForm
{
    notCrouching,
    crouchingDown,
    crouchingUp
}