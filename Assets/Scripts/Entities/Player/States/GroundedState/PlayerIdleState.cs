using UnityEngine;

[CreateAssetMenu(fileName = "Player Idle State", menuName = "States/Player/Grounded/Idle State")]

public class PlayerIdleState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;

    public override void Enter()
    {
        base.Enter();

        _player.StopMoving();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_stats.IsMovingX)
        {
            if (_stats.IsPressingCrouch)
            {
                return crouchMoveState;
            }
            else
            {
                return moveState;
            }
        }
        else if (_stats.IsPressingCrouch)
        {
            return crouchIdleState;
        }

        return null;
    }
}
