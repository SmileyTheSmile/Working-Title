using UnityEngine;

public abstract class PlayerAbilityState : PlayerState
{
    protected Movement movement
    { get => _movement ?? _entity.GetCoreComponent(ref _movement); }
    private Movement _movement;

    private bool _isGrounded => conditionManager.IsGroundedSO.value;
    protected bool _isPressingCrouch => conditionManager.IsPressingCrouchSO.value;
    protected bool _isNotMoving => conditionManager.HasStoppedFalling.value;

    protected bool _isAbilityDone;

    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    protected PlayerIdleState idleState => conditionManager.idleState;
    protected PlayerInAirState inAirState => conditionManager.inAirState;
    protected PlayerCrouchInAirState crouchInAirState => conditionManager.crouchInAirState;

    public override void Enter()
    {
        base.Enter();

        _isAbilityDone = false;
    }
    
    public override GenericState DoTransitions()
    {
        if (!_isAbilityDone)
        {
            return null;
        }
        
        if (_isGrounded && _isNotMoving)
        {
            if (_isPressingCrouch)
            {
                return crouchIdleState;
            }
            else
            {
                return idleState;
            }
        }
        else if (!_isGrounded)
        {
            if (_isPressingCrouch)
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
