using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Land State", menuName = "States/Player/Grounded/Crouch Land State")]

public class PlayerCrouchLandState : PlayerGroundedState
{
    public PlayerCrouchLandState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

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
            if (_isPressingCrouch)
            {
                stateMachine?.ChangeState(_player.crouchMoveState);
            }
            else
            {
                stateMachine?.ChangeState(_player.moveState);
            }
        }
        else
        {
            if (_isAnimationFinished)
            {
                if (_isPressingCrouch)
                {
                    stateMachine?.ChangeState(_player.crouchIdleState);
                }
                else
                {
                    stateMachine?.ChangeState(_player.idleState);
                }
            }
        }
    }
}
