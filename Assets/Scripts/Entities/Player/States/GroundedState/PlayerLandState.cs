using UnityEngine;

[CreateAssetMenu(fileName = "Player Land State", menuName = "States/Player/Grounded/Land State")]

public class PlayerLandState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;

    public override void Enter()
    {
        base.Enter();

        _player.Land();
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
        else if (_isAnimationFinished)
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

        return null;
    }
}
