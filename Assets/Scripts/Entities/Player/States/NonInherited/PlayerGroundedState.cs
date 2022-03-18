using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    #region Input Variables

    protected int inputX;
    protected int inputY;
    protected bool dashInput;
    protected bool grabInput;
    protected bool crouchInput;
    protected bool jumpInput;

    #endregion

    #region Check Variables

    protected bool isGrounded;
    protected bool isTouchingCeiling;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;

    #endregion

    #region State Functions

    public PlayerGroundedState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.collisionSenses.Ground;
        isTouchingWall = core.collisionSenses.WallFront;
        isTouchingLedge = core.collisionSenses.LedgeHorizontal;
        isTouchingCeiling = core.collisionSenses.Ceiling;
    }

    public override void Enter()
    {
        base.Enter();

        player.jumpState.ResetAmountOfJumpsLeft();
        player.dashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        inputX = player.inputHandler.normalizedInputX;
        inputY = player.inputHandler.normalizedInputY;
        grabInput = player.inputHandler.grabInput;
        dashInput = player.inputHandler.dashInput;
        jumpInput = player.inputHandler.jumpInput;
        crouchInput = player.inputHandler.crouchInput;

        if (player.inputHandler.attackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        else if (player.inputHandler.attackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.secondaryAttackState);
        }

        if (jumpInput && player.jumpState.CanJump() && !isTouchingCeiling)
        {
            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchJumpState);
            }
            else
            {
                stateMachine.ChangeState(player.jumpState);
            }
        }
        else if (!isGrounded)
        {
            player.inAirState.StartCoyoteTime();

            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchInAirState);
            }
            else
            {
                stateMachine.ChangeState(player.inAirState);
            }
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        else if (dashInput && player.dashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion
}