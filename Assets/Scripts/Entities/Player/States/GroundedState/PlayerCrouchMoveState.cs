using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Move State", menuName = "States/Player/Grounded/Crouch Move State")]

public class PlayerCrouchMoveState : PlayerGroundedState
{
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerIdleState idleState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;

    public override void Enter()
    {
        base.Enter();

        _player.Step();
        _player.Crouch();
    }

    public override void Exit()
    {
        base.Exit();

        _player.StopMovementSound();
        _player.UnCrouch();
    }

    public override void DoActions()
    {
        base.DoActions();

        _player.MoveOnGroundCrouched();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_stats.IsMovingX)
        {
            if (!_stats.IsPressingCrouch && !_stats.IsTouchingCeiling)
            {
                return moveState;
            }
        }
        else
        {
            if (!_stats.IsPressingCrouch && !_stats.IsTouchingCeiling)
            {
                return idleState;
            }
            else
            {
                return crouchIdleState;
            }
        }

        return null;
    }
}
