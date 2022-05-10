using UnityEngine;

[CreateAssetMenu(fileName = "Player Idle State", menuName = "States/Player/Grounded/Idle State")]

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityX(0f);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputX != 0f)
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
        else if (_crouchInput)
        {
            stateMachine?.ChangeState(_player.crouchIdleState);
        }
    }
}
