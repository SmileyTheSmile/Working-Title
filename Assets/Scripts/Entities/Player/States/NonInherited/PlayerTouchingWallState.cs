using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    #region Input Variables

    protected int inputX;
    protected int inputY;
    protected bool grabInput;
    protected bool jumpInput;

    #endregion

    #region Check Variables

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;
    protected bool isTouchingCeiling;

    #endregion

    #region State Functions

    public PlayerTouchingWallState(Player player, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.collisionSenses.Ground;
        isTouchingWall = core.collisionSenses.WallFront;
        isTouchingLedge = core.collisionSenses.LedgeHorizontal;
        isTouchingCeiling = core.collisionSenses.Ceiling;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        inputX = player.inputHandler.normalizedInputX;
        inputY = player.inputHandler.normalizedInputY;
        grabInput = player.inputHandler.grabInput;
        jumpInput = player.inputHandler.jumpInput;

        if (jumpInput)
        {
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.wallJumpState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (!isTouchingWall || (inputX != core.movement.facingDirection && !grabInput))
        {
            stateMachine.ChangeState(player.inAirState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.ledgeClimbState);
        }
    }

    #endregion
}