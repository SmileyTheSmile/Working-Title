using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : CoreComponent
{
    public Vector3 mousePositionInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    public bool grabInput { get; private set; }
    public bool dashInput { get; private set; }
    public bool dashInputStop { get; private set; }
    public bool crouchInput { get; private set; }
    public bool[] attackInputs { get; private set; }
    public int normalizedInputX { get; private set; }
    public int normalizedInputY { get; private set; }
    public int weaponSwitchInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;


    private PlayerInput playerInput;
    private Camera mainCamera;

    private Vector2 rawMovementInput;
    private Vector2 rawMouseInput;
    private float rawWeaponSwitchInput;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        mainCamera = Camera.main;

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        attackInputs = new bool[count];
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ProcessMouseInput();
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    //Process movement input
    public void OnMoveInput(InputAction.CallbackContext context) 
    {
        rawMovementInput = context.ReadValue<Vector2>();

        normalizedInputX = Mathf.RoundToInt(rawMovementInput.x);
        normalizedInputY = Mathf.RoundToInt(rawMovementInput.y);

        crouchInput = (normalizedInputY == -1);
    }

    //Process jump input
    public void OnJumpInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            jumpInput = true;
            jumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            jumpInputStop = true;
        }
    }

    //Process dash input
    public void OnDashInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            dashInput = true;
            dashInputStop = false;
            dashInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            dashInputStop = true;
        }
    }
    
    //Get raw mouse position input
    public void OnAimInput(InputAction.CallbackContext context) 
    {
        rawMouseInput = context.ReadValue<Vector2>();
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
            attackInputs[(int)CombatInputs.primary] = true;
        }
        else if (context.canceled)
        {
            attackInputs[(int)CombatInputs.primary] = false;
        }
    }

    //Process secondary attack input
    public void OnSecondaryAttackInput(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            attackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            attackInputs[(int)CombatInputs.secondary] = false;
        }
    }

    //Process weapon switch input
    public void OnWeaponSwitchInput(InputAction.CallbackContext context) 
    {
        rawWeaponSwitchInput = context.ReadValue<Vector2>().y;

        weaponSwitchInput = Math.Sign(rawWeaponSwitchInput);
    }
    
    //Check if jump button has been held for the value in inputHoldTime
    private void CheckJumpInputHoldTime() 
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }

    //Check if dash button has been held for value in inputHoldTime
    private void CheckDashInputHoldTime() 
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            dashInput = false;
        }
    }
    
    //Process mouse position input
    private void ProcessMouseInput() 
    {
        mousePositionInput = new Vector3(rawMouseInput.x, rawMouseInput.y, 10);

        mousePositionInput = mainCamera.ScreenToWorldPoint(mousePositionInput);
        //mouseAngle = Mathf.Atan2(mousePositionInput.y, mousePositionInput.x) * Mathf.Rad2Deg;
    }

    //Disable jump input
    public void UseJumpInput() => jumpInput = false; 
    //Disable dash input
    public void UseDashInput() => dashInput = false; 

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
