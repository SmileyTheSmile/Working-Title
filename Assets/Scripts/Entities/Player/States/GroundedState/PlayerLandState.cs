using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    #region State Functions

    public PlayerLandState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0)
        {
            if (inputY != -1)
            {
                stateMachine.ChangeState(player.moveState);
            }
            else
            {
                stateMachine.ChangeState(player.crouchMoveState);
            }
        }
        else if (isAnimationFinished)
        {
            if (inputY != -1)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
        }
    }

    #endregion
}
