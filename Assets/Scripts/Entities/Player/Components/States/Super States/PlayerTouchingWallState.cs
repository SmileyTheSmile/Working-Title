using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    protected int inputX;
    protected int inputY;
    protected bool grabInput;
    protected bool jumpInput;
    protected bool crouchInput;

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;
    protected bool isTouchingCeiling;

    public PlayerTouchingWallState(Player player, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenses.Ground;
        isTouchingWall = collisionSenses.WallFront;
        isTouchingLedge = collisionSenses.LedgeHorizontal;
        isTouchingCeiling = collisionSenses.Ceiling;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        inputX = inputHandler.normalizedInputX;
        inputY = inputHandler.normalizedInputY;
        grabInput = inputHandler.grabInput;
        jumpInput = inputHandler.jumpInput;
        crouchInput = inputHandler.crouchInput;

        if (jumpInput)
        {
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine?.ChangeState(player.wallJumpState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine?.ChangeState(player.idleState);
        }
        else if (isGrounded && crouchInput && !isTouchingCeiling)
        {
            stateMachine?.ChangeState(player.crouchIdleState);
        }
        else if (!isTouchingWall || (inputX != movement.facingDirection && !grabInput))
        {
            stateMachine?.ChangeState(player.inAirState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isTouchingCeiling)
        {
            stateMachine?.ChangeState(player.ledgeClimbState);
        }
    }
}
