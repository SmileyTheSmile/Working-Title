using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Module that processes player input.
/// </summary>>
[CreateAssetMenu(fileName = "InputHandler", menuName = "ScriptableObjects/Input Handler")]
public class PlayerInputHandler : ScriptableObject, PlayerInputActions.IGameplayActions, PlayerInputActions.IUIActions
{
    private PlayerInputActions _playerInputActions = default;

    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2> AimEvent = delegate { };
    public event UnityAction<Vector2> WeaponSwitchEvent = delegate { };
    public event UnityAction<bool> PauseEvent = delegate { };
    public event UnityAction<bool> GrabEvent = delegate { };
    public event UnityAction<bool> CrouchEvent = delegate { };
    public event UnityAction<bool> PrimaryAttackEvent = delegate { };
    public event UnityAction<bool> SecondaryAttackEvent = delegate { };
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCanceledEvent = delegate { };

    public void OnEnable()
    {
        if (_playerInputActions == default)
        {
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Gameplay.SetCallbacks(this);
            //_playerInputActions.UI.SetCallbacks(this);
        }

        _playerInputActions.Enable();
    }

    public void OnDisable()
    {
        _playerInputActions.Gameplay.Disable();
        _playerInputActions.UI.Disable();
    }

    //Gameplay input
    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            JumpEvent.Invoke();
        else if (context.canceled)
            JumpCanceledEvent.Invoke();
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started)
            GrabEvent.Invoke(true);
        else if (context.canceled)
            GrabEvent.Invoke(false);
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            PrimaryAttackEvent.Invoke(true);
        else if (context.canceled)
            PrimaryAttackEvent.Invoke(false);
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            SecondaryAttackEvent.Invoke(true);
        else if (context.canceled)
            SecondaryAttackEvent.Invoke(false);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
            CrouchEvent.Invoke(true);
        else if (context.canceled)
            CrouchEvent.Invoke(false);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnWeaponSwitch(InputAction.CallbackContext context)
    {
        WeaponSwitchEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
            PauseEvent.Invoke(true);
        else if (context.canceled)
            PauseEvent.Invoke(false);
    }

    //UI input
    public void OnNavigate(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}