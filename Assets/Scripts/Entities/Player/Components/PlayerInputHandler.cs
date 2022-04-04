using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 rawMovementInput { get; private set; }
    public Vector2 rawMouseInput { get; private set; }
    public Vector3 mousePositionInput { get; private set; }

    public float rawWeaponSwitchInput { get; private set; }
    public float mouseAngle { get; private set; }

    private PlayerInput playerInput;

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

    private Camera mainCamera;

    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        mainCamera = Camera.main;

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        attackInputs = new bool[count];
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context) //Process WASD input
    {
        rawMovementInput = context.ReadValue<Vector2>();

        normalizedInputX = Mathf.RoundToInt(rawMovementInput.x);
        normalizedInputY = Mathf.RoundToInt(rawMovementInput.y);

        crouchInput = (normalizedInputY == -1);
    }

    public void OnJumpInput(InputAction.CallbackContext context) //Process jump input
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

    public void OnDashInput(InputAction.CallbackContext context) //Process dash input
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

    public void OnDashDirectionInput(InputAction.CallbackContext context) //Process mouse position input
    {
        rawMouseInput = context.ReadValue<Vector2>();

        mousePositionInput = new Vector3(rawMouseInput.x, rawMouseInput.y, 10);

        mousePositionInput = mainCamera.ScreenToWorldPoint(mousePositionInput);
        //mouseAngle = Mathf.Atan2(mousePositionInput.y, mousePositionInput.x) * Mathf.Rad2Deg;
    }

    public void OnGrabInput(InputAction.CallbackContext context) //Process wall grab input
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

    public void OnCrouchInput(InputAction.CallbackContext context) //Process crouch input
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

    public void OnPrimaryAttackInput(InputAction.CallbackContext context) //Process primary attack input
    {
        if (context.started)
        {
            attackInputs[(int)CombatInputs.primary] = true;
        }

        if (context.canceled)
        {
            attackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context) //Process secondary attack input
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

    public void OnWeaponSwitchInput(InputAction.CallbackContext context) //Process weapon switch input
    {
        rawWeaponSwitchInput = context.ReadValue<Vector2>().y;

        weaponSwitchInput = Math.Sign(rawWeaponSwitchInput);
    }

    public void CheckJumpInputHoldTime() //Check if jump button has been held for value in inputHoldTime
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }

    public void CheckDashInputHoldTime() //Check if dash button has been held for value in inputHoldTime
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            dashInput = false;
        }
    }

    public void UseJumpInput() => jumpInput = false; //Disable jump input

    public void UseDashInput() => dashInput = false; //Disable dash input

    public void LogAllInputs() //Process crouch input
    {
        Debug.Log($"Crouch = {crouchInput}, Jump = {jumpInput}, Weapon Switch = {weaponSwitchInput}");
    }
}

public enum CombatInputs
{
    primary,
    secondary
}
