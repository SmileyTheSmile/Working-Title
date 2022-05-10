using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Grab State", menuName = "States/Player/Touching Wall/Wall Grab State")]

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 _holdPosition;

    public PlayerWallGrabState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        _holdPosition = _player.transform.position;

        HoldPosition();
    }

    public override void DoActions()
    {
        base.DoActions();

        HoldPosition();
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_inputY > 0)
        {
            stateMachine.ChangeState(_player.wallClimbState);
        }
        else if (_inputY < 0 || !_grabInput)
        {
            stateMachine.ChangeState(_player.wallSlideState);
        }
    }

    private void HoldPosition()
    {
        _player.transform.position = _holdPosition;

        movement?.SetVelocityZero();
    }
}
