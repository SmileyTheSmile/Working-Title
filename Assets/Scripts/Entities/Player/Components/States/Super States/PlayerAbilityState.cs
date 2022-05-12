using UnityEngine;

public abstract class PlayerAbilityState : PlayerState
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }
    private CollisionSenses _collisionSenses;

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    private bool _isGrounded => collisionSenses._groundCheck.value;

    protected bool _isAbilityDone;
    protected bool _crouchInput;

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

    public override void DoActions()
    {
        base.DoActions();

        _crouchInput = inputHandler.crouchInput;
    }
    
    public override void DoTransitions()
    {
        base.DoTransitions();

        if (_isGrounded && movement._currentVelocity.y < 0.01)
        {
            if (_crouchInput)
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
            if (_crouchInput)
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
