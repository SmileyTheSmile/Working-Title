using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Move State", menuName = "States/Player/Grounded/Crouch Move State")]

public class PlayerCrouchMoveState : PlayerGroundedState
{
    public PlayerCrouchMoveState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        _crouchInput = inputHandler.crouchInput;

        movement?.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }

    public override void Exit()
    {
        base.Exit();

        _crouchInput = inputHandler.crouchInput;

        movement?.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }

    public override void DoActions()
    {
        base.DoActions();

        movement?.SetVelocityX(_playerData.crouchMovementVelocity * movement._movementDir);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputX != 0f)
        {
            if (!_crouchInput && !_isTouchingCeiling)
            {
                stateMachine?.ChangeState(_player.moveState);
            }
        }
        else
        {
            if (!_crouchInput && !_isTouchingCeiling)
            {
                stateMachine?.ChangeState(_player.idleState);
            }
            else
            {
                stateMachine?.ChangeState(_player.crouchIdleState);
            }
        }
    }
}
