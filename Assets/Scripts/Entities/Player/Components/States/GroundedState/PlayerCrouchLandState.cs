using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Land State", menuName = "States/Player/Grounded/Crouch Land State")]

public class PlayerCrouchLandState : PlayerGroundedState
{
    public PlayerCrouchLandState(Player player, PlayerData playerData, string animBoolName)
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

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputX != 0)
        {
            if (_crouchInput)
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
                if (_crouchInput)
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
