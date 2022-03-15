using System.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera camera;

    public Vector2 rawMovementInput { get; private set; }
    public Vector2 rawDashDirectionInput { get; private set; }

    public Vector2 mouseDirectionInput { get; private set; }

    public int normalizedInputX { get; private set; }
    public int normalizedInputY { get; private set; }

    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    public bool grabInput { get; private set; }
    public bool dashInput { get; private set; }
    public bool dashInputStop { get; private set; }
    public bool[] attackInputs { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        attackInputs = new bool[count];

        camera = Camera.main;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawMovementInput = context.ReadValue<Vector2>();

        normalizedInputX = Mathf.RoundToInt(rawMovementInput.x);
        normalizedInputY = Mathf.RoundToInt(rawMovementInput.y);


    }

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

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        rawDashDirectionInput = context.ReadValue<Vector2>();

        mouseDirectionInput = new Vector2(rawDashDirectionInput.x - (Screen.width / 2), rawDashDirectionInput.y - (Screen.height / 2));
    }

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

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
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

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }

    public void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            dashInput = false;
        }
    }

    public void UseJumpInput() => jumpInput = false;

    public void UseDashInput() => dashInput = false;

}

public enum CombatInputs
{
    primary,
    secondary
}
