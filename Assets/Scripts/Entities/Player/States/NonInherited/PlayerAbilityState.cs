using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    #region Check Variables

    private bool isGrounded;

    #endregion

    #region Utility Variables
    protected bool isAbilityDone;
    protected bool crouchInput;

    #endregion

    #region State Functions

    public PlayerAbilityState(Player player, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.collisionSenses.Ground;
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

        if (isGrounded && core.movement.currentVelocity.y < 0.01)
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

    #endregion
}
