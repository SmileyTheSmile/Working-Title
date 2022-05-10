using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Idle State", menuName = "States/Player/Grounded/Crouch Idle State")]

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public PlayerCrouchIdleState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityZero();

        _crouchInput = inputHandler.crouchInput;

        movement?.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }

    public override void Exit()
    {
        base.Exit();

        _crouchInput = inputHandler.crouchInput;

        movement?.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputX != 0)
        {
            if (_crouchInput || _isTouchingCeiling)
            {
                stateMachine?.ChangeState(_player.crouchMoveState);
            }
            else
            {

                stateMachine?.ChangeState(_player.moveState);
            }
        }
        else if (!_crouchInput && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.idleState);
        }
    }
}
