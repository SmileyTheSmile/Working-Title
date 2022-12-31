using UnityEngine;
using UnityEngine.InputSystem;
using Platformer.Input;

namespace Platformer.Entity.ConditionsSystem
{
    /// <summary>
    /// Component of an entity that connects to the player's "Gameplay" action map.
    /// </summary>>
    public class GameplayInput : MonoBehaviour, IEntityComponent, PlayerInputActions.IGameplayActions
    {
        [SerializeField]
        private PlayerInputActions _playerInputActions = default;

        public void OnEnable()
        {
            if (_playerInputActions == default)
            {
                _playerInputActions = new PlayerInputActions();
                _playerInputActions.Gameplay.SetCallbacks(this);
            }

            _playerInputActions.Enable();
        }

        public void OnDisable()
        {
            _playerInputActions.Gameplay.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            return;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
                return;
            else if (context.canceled)
                return;
        }

        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.started)
                return;
            else if (context.canceled)
                return;
        }

        public void OnPrimaryAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                return;
            else if (context.canceled)
                return;
        }

        public void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                return;
            else if (context.canceled)
                return;
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started)
                return;
            else if (context.canceled)
                return;
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            return;
        }

        public void OnWeaponSwitch(InputAction.CallbackContext context)
        {
            return;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started)
                return;
            else if (context.canceled)
                return;
        }
    }
}