using UnityEngine;

[CreateAssetMenu(fileName = "Player Land State", menuName = "States/Player/Grounded/Land State")]

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

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
