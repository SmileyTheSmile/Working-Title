using UnityEngine;

[CreateAssetMenu(fileName = "Player Move State", menuName = "States/Player/Grounded/Move State")]

public class PlayerMoveState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;

    public override void Enter()
    {
        base.Enter();

        _player.Step();
    }

    public override void Exit()
    {
        base.Exit();

        _player.StopMovementSound();
    }

    public override void DoActions()
    {
        base.DoActions();

        _player.MoveOnGround();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (!_stats.IsMovingX)
        {
            if (_stats.IsPressingCrouch)
            {
                return crouchIdleState;
            }
            else
            {
                return idleState;
            }
        }
        else if (_stats.IsPressingCrouch) {
            return crouchMoveState;
        }

        return null;
    }
}