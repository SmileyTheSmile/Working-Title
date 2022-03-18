using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    #region Check Variables

    private bool isGrounded;

    #endregion

    #region Utility Variables
    protected bool isAbilityDone;

    #endregion

    #region State Functions

    public PlayerAbilityState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
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

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isAbilityDone)
        {
            return;
        }

        if (isGrounded && core.movement.currentVelocity.y < 0.01)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            stateMachine.ChangeState(player.inAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion
}
