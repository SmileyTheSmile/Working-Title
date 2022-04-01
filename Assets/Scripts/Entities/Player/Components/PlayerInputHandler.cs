using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Raw Input Variables

    public Vector2 rawMovementInput { get; private set; }
    public Vector2 rawDashDirectionInput { get; private set; }

    public float rawWeaponSwitchInput { get; private set; }

    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    public bool grabInput { get; private set; }
    public bool dashInput { get; private set; }
    public bool dashInputStop { get; private set; }
    public bool crouchInput { get; private set; }
    public bool[] attackInputs { get; private set; }

    #endregion

    #region Processed Input Variables

    public Vector2 mouseDirectionInput { get; private set; }

    public int normalizedInputX { get; private set; }
    public int normalizedInputY { get; private set; }
    public int weaponSwitchInput { get; private set; }

    #endregion

    #region Input System Components

    private PlayerInput playerInput;

    #endregion

    #region Utility Variables

    [SerializeField]
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    #endregion

    #region Unity Functions

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        attackInputs = new bool[count];
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    #endregion

    #region Input Processing Functions

    public void OnMoveInput(InputAction.CallbackContext context) //Process WASD input
    {
        rawMovementInput = context.ReadValue<Vector2>();

        normalizedInputX = Mathf.RoundToInt(rawMovementInput.x);
        normalizedInputY = Mathf.RoundToInt(rawMovementInput.y);

        if (normalizedInputY == -1)
        {
            crouchInput = true;
        }
        else
        {
            crouchInput = false;
        }
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
        rawDashDirectionInput = context.ReadValue<Vector2>();

        mouseDirectionInput = new Vector2(rawDashDirectionInput.x - (Screen.width / 2), rawDashDirectionInput.y - (Screen.height / 2));
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

    #endregion

    #region Utility Functions

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

    #endregion

    #region Editor Functions

    public void LogAllInputs() //Process crouch input
    {
        Debug.Log($"Crouch = {crouchInput}, Jump = {jumpInput}, Weapon Switch = {weaponSwitchInput}");
    }

    #endregion
}

public enum CombatInputs
{
    primary,
    secondary
}
