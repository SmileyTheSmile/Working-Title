using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : CoreComponent
{
    public Vector3 mousePositionInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    public bool grabInput { get; private set; }
    public bool crouchInput { get; private set; }
    public bool[] _attackInputs { get; private set; }
    public int normalizedInputX { get; private set; }
    public int normalizedInputY { get; private set; }
    public int weaponSwitchInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;
    [SerializeField] private PlayerData _playerData;

    private PlayerInput _playerInput;
    private Camera _mainCamera;

    private Vector2 _rawMovementInput;
    private Vector2 _rawMouseInput;
    private float _rawWeaponSwitchInput;
    private float _jumpInputStartTime;
    private int _amountOfJumpsLeft;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _mainCamera = Camera.main;

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        _attackInputs = new bool[count];
        _amountOfJumpsLeft = _playerData.amountOfJumps;
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

        crouchInput = (normalizedInputY == -1);
    }

    //Process jump input
    public void OnJumpInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            jumpInput = true;
            jumpInputStop = false;
            _jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            jumpInputStop = true;
        }
    }
    
    //Get raw mouse position input
    public void OnAimInput(InputAction.CallbackContext context) 
    {
        _rawMouseInput = context.ReadValue<Vector2>();
    }

    //Process wall grab input
    public void OnGrabInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            grabInput = true;
        }

        if (context.canceled)
        {
            grabInput = false;
        }
    }

    //Process crouch input
    public void OnCrouchInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            crouchInput = true;
        }

        if (context.canceled)
        {
            crouchInput = false;
        }
    }

    //Process primary attack input
    public void OnPrimaryAttackInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            _attackInputs[(int)CombatInputs.primary] = true;
        }
        else if (context.canceled)
        {
            _attackInputs[(int)CombatInputs.primary] = false;
        }
    }

    //Process secondary attack input
    public void OnSecondaryAttackInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            _attackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            _attackInputs[(int)CombatInputs.secondary] = false;
        }
    }

    //Process weapon switch input
    public void OnWeaponSwitchInput(InputAction.CallbackContext context) 
    {
        _rawWeaponSwitchInput = context.ReadValue<Vector2>().y;

        weaponSwitchInput = (int)_rawWeaponSwitchInput;
        Debug.Log(weaponSwitchInput);
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

public enum CombatInputs
{
    primary,
    secondary
}
