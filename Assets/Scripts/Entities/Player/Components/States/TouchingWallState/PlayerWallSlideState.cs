using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isExitingState)
        {
            return;
        }

        movement?.SetVelocityY(-playerData.wallSlideVelocity);

        if (grabInput && inputY == 0)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
    }
}