using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : CoreComponent
{
    [SerializeField] private InputTransitionCondition _isPressingJumpSO;
    [SerializeField] private InputTransitionCondition _isJumpCanceledSO;
    [SerializeField] private InputTransitionCondition _isPressingGrabSO;
    [SerializeField] private InputTransitionCondition _isCrouchingSO;
    [SerializeField] private InputTransitionCondition _isPressingPrimaryAttackSO;
    [SerializeField] private InputTransitionCondition _isPressingSecondaryAttackSO;
    [SerializeField] private InputTransitionCondition _isPressingPauseSO;

    public ScriptableInt _normalizedInputXSO;
    public ScriptableInt _normalizedInputYSO;
    public ScriptableInt _weaponSwitchInputSO;
    public ScriptableVector3 _mousePositionInputSO;

    [SerializeField] private PlayerData _playerData;

    private PlayerInput _playerInput;
    private Camera _mainCamera;

    private Vector2 _rawMouseInput;
    private float _jumpInputStartTime;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _mainCamera = Camera.main;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ProcessMouseInput();
        CheckJumpInputHoldTime();
    }

    //Process movement input
    public void OnMoveInput(InputAction.CallbackContext context) 
    {
        Vector2 rawMovementInput = context.ReadValue<Vector2>();

        _normalizedInputXSO.value = Mathf.RoundToInt(rawMovementInput.x);

        _normalizedInputYSO.value = Mathf.RoundToInt(rawMovementInput.y);
    }

    //Process jump input
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPressingJumpSO.value = true;
            _isJumpCanceledSO.value = false;

            _jumpInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            _isPressingJumpSO.value = false;
            _isJumpCanceledSO.value = true;
        }
    }

    //Process wall grab input
    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPressingGrabSO.value = true;
        }
        else if (context.canceled)
        {
            _isPressingGrabSO.value = false;
        }
    }

    //Process crouch input
    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isCrouchingSO.value = true;
        }
        else if (context.canceled)
        {
            _isCrouchingSO.value = false;
        }
    }

    //Process primary attack input
    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPressingPrimaryAttackSO.value = true;
        }
        else if (context.canceled)
        {
            _isPressingPrimaryAttackSO.value = false;
        }
    }

    //Process secondary attack input
    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPressingSecondaryAttackSO.value = true;
        }
        else if (context.canceled)
        {
            _isPressingSecondaryAttackSO.value = false;
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
        _weaponSwitchInputSO.value = (int)context.ReadValue<Vector2>().y;
    }

    //Process pause input
    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isPressingPauseSO.value = true;
        }
        else if (context.canceled)
        {
            _isPressingPauseSO.value = false;
        }
    }
    
    //Check if jump button has been held for the value in inputHoldTime
    private void CheckJumpInputHoldTime() 
    {
        if (Time.time >= _jumpInputStartTime + _playerData.jumpInputHoldTime)
        {
            _isPressingJumpSO.value = false;
        }
    }
    
    //Process mouse position input
    private void ProcessMouseInput() 
    {
        Vector3 shiftedMouseInput = new Vector3(_rawMouseInput.x, _rawMouseInput.y, 10);

        _mousePositionInputSO.value = _mainCamera.ScreenToWorldPoint(shiftedMouseInput);
    }

    public void UseJumpInput() => _isPressingJumpSO.value = false; 
}