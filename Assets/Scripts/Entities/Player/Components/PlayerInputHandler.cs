using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : CoreComponent
{
    [SerializeField] private InputTransitionCondition _isJumpingSO;
    [SerializeField] private InputTransitionCondition _isJumpCanceledSO;
    [SerializeField] private InputTransitionCondition _isGrabbingSO;
    [SerializeField] private InputTransitionCondition _isCrouchingSO;
    [SerializeField] private InputTransitionCondition _isMovingSO;
    [SerializeField] private InputTransitionCondition _primaryAttackSO;
    [SerializeField] private InputTransitionCondition _secondaryAttackSO;

    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    public bool grabInput { get; private set; }
    public bool crouchInput { get; private set; }
    public bool primaryAttackInput { get; private set; }
    public bool secondaryAttackInput { get; private set; }
    public int normalizedInputX { get; private set; }
    public int normalizedInputY { get; private set; }
    public int weaponSwitchInput { get; private set; }
    public Vector3 mousePositionInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;
    [SerializeField] private PlayerData _playerData;

    private PlayerInput _playerInput;
    private Camera _mainCamera;

    private Vector2 _rawMovementInput;
    private Vector2 _rawMouseInput;
    private float _rawWeaponSwitchInput;
    private float _jumpInputStartTime;
    private int _amountOfJumpsLeft;
    private int _amountOfCrouchesLeft;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _mainCamera = Camera.main;

        ResetAmountOfJumpsLeft();
        ResetAmountOfCrouchesLeft();
    }

    private void Update()
    {
        base.LogicUpdate();

        ProcessMouseInput();
        CheckJumpInputHoldTime();
    }

    //Process movement input
    public void OnMoveInput(InputAction.CallbackContext context) 
    {
        _rawMovementInput = context.ReadValue<Vector2>();

        normalizedInputX = Mathf.RoundToInt(_rawMovementInput.x);
        normalizedInputY = Mathf.RoundToInt(_rawMovementInput.y);

        if (normalizedInputX != 0)
            _isMovingSO.value = true;
    }

    //Process jump input
    public void OnJumpInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            _isJumpingSO.value = true;
            _isJumpCanceledSO.value = false;

            jumpInput = true;
            jumpInputStop = false;

            _jumpInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            _isJumpingSO.value = false;
            _isJumpCanceledSO.value = true;

            jumpInput = false;
            jumpInputStop = true;
        }
    }

    //Process wall grab input
    public void OnGrabInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            _isGrabbingSO.value = true;
            grabInput = true;
        }
        else if (context.canceled)
        {
            _isGrabbingSO.value = false;
            grabInput = false;
        }
    }

    //Process crouch input
    public void OnCrouchInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            _isCrouchingSO.value = true;
            crouchInput = true;
        }
        else if (context.canceled)
        {
            _isCrouchingSO.value = false;
            crouchInput = false;
        }
    }

    //Process primary attack input
    public void OnPrimaryAttackInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            _primaryAttackSO.value = true;
            primaryAttackInput = true;
        }
        else if (context.canceled)
        {
            _primaryAttackSO.value = false;
            primaryAttackInput = false;
        }
    }

    //Process secondary attack input
    public void OnSecondaryAttackInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            _secondaryAttackSO.value = true;
            secondaryAttackInput = true;
        }
        else if (context.canceled)
        {
            _secondaryAttackSO.value = false;
            secondaryAttackInput = false;
        }
    }

    //Get raw mouse position input
    public void OnAimInput(InputAction.CallbackContext context)
    {
        _rawMouseInput = context.ReadValue<Vector2>();
    }

    //Process weapon switch input
    public void OnWeaponSwitchInput(InputAction.CallbackContext context) 
    {
        _rawWeaponSwitchInput = context.ReadValue<Vector2>().y;

        weaponSwitchInput = (int)_rawWeaponSwitchInput;
    }
    
    //Check if jump button has been held for the value in inputHoldTime
    private void CheckJumpInputHoldTime() 
    {
        if (Time.time >= _jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }
    
    //Process mouse position input
    private void ProcessMouseInput() 
    {
        mousePositionInput = new Vector3(_rawMouseInput.x, _rawMouseInput.y, 10);

        mousePositionInput = _mainCamera.ScreenToWorldPoint(mousePositionInput);
        //mouseAngle = Mathf.Atan2(mousePositionInput.y, mousePositionInput.x) * Mathf.Rad2Deg;
    }

    public bool CanCrouch() => (_amountOfCrouchesLeft > 0);
    public void ResetAmountOfCrouchesLeft() => _amountOfCrouchesLeft = _playerData.amountOfCrouches;
    public void DecreaseAmountOfCrouchesLeft() => _amountOfCrouchesLeft--;

    public bool CanJump() => (_amountOfJumpsLeft > 0);
    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.amountOfJumps;
    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
    //Disable jump input
    public void UseJumpInput() => jumpInput = false; 

    //Log important info
    public override void LogComponentInfo() 
    {
        Debug.Log($"Crouch = {crouchInput}, Jump = {jumpInput}, Weapon Switch = {weaponSwitchInput}");
    }
}