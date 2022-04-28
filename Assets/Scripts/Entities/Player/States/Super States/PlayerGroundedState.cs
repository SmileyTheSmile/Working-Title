using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }

    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;

    protected int inputX;
    protected int inputY;
    protected bool dashInput;
    protected bool grabInput;
    protected bool crouchInput;
    protected bool jumpInput;
    protected bool jumpInputStop;
    protected Vector2 mousePositionInput;

    protected bool isGrounded;
    protected bool isCrouching;
    protected bool isTouchingCeiling;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;

    public PlayerGroundedState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        if (collisionSenses)
        {
            isGrounded = collisionSenses.Ground;
            isTouchingCeiling = collisionSenses.Ceiling;
            isTouchingWall = collisionSenses.WallFront;
            isTouchingLedge = collisionSenses.LedgeHorizontal;
        }
    }

    public override void Enter()
    {
        base.Enter();

        player.jumpState.ResetAmountOfJumpsLeft();
        player.crouchInAirState.ResetAmountOfCrouchesLeft();
        player.dashState.ResetCanDash();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        inputX = player.inputHandler.normalizedInputX;
        inputY = player.inputHandler.normalizedInputY;
        grabInput = player.inputHandler.grabInput;
        dashInput = player.inputHandler.dashInput;
        jumpInput = player.inputHandler.jumpInput;
        jumpInputStop = player.inputHandler.jumpInputStop;
        crouchInput = player.inputHandler.crouchInput;
        mousePositionInput = player.inputHandler.mousePositionInput;

        movement.CheckIfShouldFlip(inputX, mousePositionInput.x);

        //Ability States
        if (player.inputHandler.attackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            //stateMachine.ChangeState(player.primaryAttackState);
        }
        else if (player.inputHandler.attackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.secondaryAttackState);
        }
        else if ((jumpInput && player.jumpState.CanJump() && !isTouchingCeiling))
        {
            if (crouchInput && player.crouchInAirState.CanCrouch())
            {
                stateMachine.ChangeState(player.crouchJumpState);
            }
            else
            {
                stateMachine.ChangeState(player.jumpState);
            }
        }
        else if (dashInput && player.dashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.dashState);
        }

        //Other States
        if (!isGrounded)
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
        else if (isTouchingWall && grabInput && isTouchingLedge && !isTouchingCeiling && !crouchInput)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
    }
}