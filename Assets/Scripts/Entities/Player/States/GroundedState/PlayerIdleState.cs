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

        _movement.SetVelocityX(0f);
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_conditions.IsMovingX) {
            if (_conditions.IsPressingCrouch) {
                return crouchMoveState;
            } else {
                return moveState;
            }
        } else if (_conditions.IsPressingCrouch) {
            return crouchIdleState;
        }

        return null;
    }
}
