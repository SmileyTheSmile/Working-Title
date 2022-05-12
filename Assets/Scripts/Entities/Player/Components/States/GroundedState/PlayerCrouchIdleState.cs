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

        movement?.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();

        movement?.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputX != 0)
        {
            if (_isPressingCrouch || _isTouchingCeiling)
            {
                stateMachine?.ChangeState(_player.crouchMoveState);
            }
            else
            {

                stateMachine?.ChangeState(_player.moveState);
            }
        }
        else if (!_isPressingCrouch && !_isTouchingCeiling)
        {
            stateMachine?.ChangeState(_player.idleState);
        }
    }
}
