using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Idle State", menuName = "States/Player/Grounded/Crouch Idle State")]

public class PlayerCrouchIdleState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerIdleState idleState;

    public override void Enter()
    {
        base.Enter();

        _player.StopMoving();
        _player.Crouch();
    }

    public override void Exit()
    {
        base.Exit();

        _player.UnCrouch();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_stats.IsMovingX)
        {
            if (_stats.IsPressingCrouch || _stats.IsTouchingCeiling)
            {
                return crouchMoveState;
            }
            else
            {
                return moveState;
            }
        }
        else if (!_stats.IsPressingCrouch && !_stats.IsTouchingCeiling)
        {
            return idleState;
        }

        return null;
    }
}
