using UnityEngine;

public abstract class PlayerAbilityState : PlayerState
{
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] protected PlayerCrouchInAirState crouchInAirState;

    protected bool _isAbilityDone;
    
    public override void Enter()
    {
        base.Enter();

        _isAbilityDone = true;
    }
    
    public override GenericState DoTransitions()
    {
        if (!_isAbilityDone)
        {
            return null;
        }
        
        if (_conditions.IsGrounded && _conditions.HasStoppedFalling)
        {
            if (_conditions.IsPressingCrouch)
            {
                return crouchIdleState;
            }
            else
            {
                return idleState;
            }
        }
        else if (!_conditions.IsGrounded)
        {
            if (_conditions.IsPressingCrouch)
            {
                return crouchInAirState;
            }
            else
            {
                return inAirState;
            }
        }

        return null;
    }
}
