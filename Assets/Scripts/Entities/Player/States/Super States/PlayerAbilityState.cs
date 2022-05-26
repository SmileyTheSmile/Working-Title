using UnityEngine;

public abstract class PlayerAbilityState : PlayerState
{
    protected Movement movement
    { get => _movement ?? _entity.GetCoreComponent(ref _movement); }
    private Movement _movement;

    [SerializeField] protected CollisionCheckTransitionCondition IsGroundedSO;
    [SerializeField] protected InputTransitionCondition IsPressingCrouchSO;
    [SerializeField] protected SupportTransitionCondition HasStoppedFallingSO;

    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;
    [SerializeField] protected PlayerInAirState inAirState;
    [SerializeField] protected PlayerCrouchInAirState crouchInAirState;

    protected bool _isGrounded => IsGroundedSO.value;
    protected bool _isPressingCrouch => IsPressingCrouchSO.value;
    protected bool _hasStoppedFalling => HasStoppedFallingSO.value;

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
        
        if (_isGrounded && _hasStoppedFalling)
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
