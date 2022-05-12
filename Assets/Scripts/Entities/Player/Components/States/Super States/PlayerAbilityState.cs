using UnityEngine;

public abstract class PlayerAbilityState : PlayerState
{
    protected ConditionManager conditionManager
    { get => _conditionManager ?? core.GetCoreComponent(ref _conditionManager); }
    private ConditionManager _conditionManager;

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    private bool _isGrounded => conditionManager.IsGroundedSO.value;
    protected bool _isPressingCrouch => conditionManager.IsPressingCrouchSO.value;

    protected bool _isAbilityDone;

    public PlayerAbilityState(Player player, string animBoolName, PlayerData playerData)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        _isAbilityDone = false;
    }

    public override void LogicUpdate()
    {
        if (_isExitingState)
        {
            return;
        }
        
        if (!_isAbilityDone)
        {
            return;
        }

        DoActions();
        DoTransitions();
    }
    
    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_isGrounded && movement._currentVelocity.y < 0.01)
        {
            if (_isPressingCrouch)
            {
                stateMachine?.ChangeState(_player.crouchIdleState);
            }
            else
            {
                stateMachine?.ChangeState(_player.idleState);
            }
        }
        else if (!_isGrounded)
        {
            if (_isPressingCrouch)
            {
                stateMachine?.ChangeState(_player.crouchInAirState);
            }
            else
            {
                stateMachine?.ChangeState(_player.inAirState);
            }
        }
    }
}
