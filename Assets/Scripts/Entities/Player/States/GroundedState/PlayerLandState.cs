using UnityEngine;

[CreateAssetMenu(fileName = "Player Land State", menuName = "States/Player/Grounded/Land State")]

public class PlayerLandState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;

    protected AudioSourcePlayer _fallSound => _temporaryComponent.fallSound;

    public override void Enter()
    {
        base.Enter();

        Step();
    }

    protected virtual void Step()
    {
        if (_fallSound)
            _fallSound.Play();
    }
    
    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (_conditions.IsMovingX)
        {
            if (_conditions.IsPressingCrouch) {
                return crouchMoveState;
            } else {
                return moveState;
            }
        } else if (_isAnimationFinished) {
            if (_conditions.IsPressingCrouch) {
                return crouchIdleState;
            } else {
                return idleState;
            }
        }

        return null;
    }
}
