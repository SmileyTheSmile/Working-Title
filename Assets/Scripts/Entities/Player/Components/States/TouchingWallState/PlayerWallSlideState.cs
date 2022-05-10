using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Slide State", menuName = "States/Player/Touching Wall/Wall Slide State")]

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void DoActions()
    {
        base.DoActions();

        movement?.SetVelocityY(-_playerData.wallSlideVelocity);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_grabInput && _inputY == 0)
        {
            stateMachine.ChangeState(_player.wallGrabState);
        }
    }
}