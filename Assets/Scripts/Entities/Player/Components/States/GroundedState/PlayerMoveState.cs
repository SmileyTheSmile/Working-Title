using UnityEngine;

[CreateAssetMenu(fileName = "Player Move State", menuName = "States/Player/Grounded/Move State")]

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

    public override void DoActions()
    {
        base.DoActions();

        movement?.SetVelocityX(_playerData.movementVelocity * _inputX);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputX == 0f)
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
        else if (_isPressingCrouch)
        {
            stateMachine?.ChangeState(_player.crouchMoveState);
        }
    }
}