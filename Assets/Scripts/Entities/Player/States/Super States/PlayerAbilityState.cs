using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    private bool isGrounded;

    protected bool isAbilityDone;
    protected bool crouchInput;

    public PlayerAbilityState(Player player, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isAbilityDone)
        {
            return;
        }

        crouchInput = player.inputHandler.crouchInput;

        if (isGrounded && movement.currentVelocity.y < 0.01)
        {
            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
        else if (!isGrounded)
        {
            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchInAirState);
            }
            else
            {
                stateMachine.ChangeState(player.inAirState);
            }
        }
    }
}
