using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isExitingState)
        {
            return;
        }

        movement?.SetVelocityY(playerData.wallClimbVelocity);

        if (inputY != 1)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
    }
}
