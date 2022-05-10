using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Climb State", menuName = "States/Player/Touching Wall/Wall Climb State")]

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void DoActions()
    {
        base.DoActions();

        movement?.SetVelocityY(_playerData.wallClimbVelocity);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputY != 1)
        {
            stateMachine.ChangeState(_player.wallGrabState);
        }
    }
}
