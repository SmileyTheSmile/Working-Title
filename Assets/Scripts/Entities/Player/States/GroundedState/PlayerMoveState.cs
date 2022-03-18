using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    #region State Functions

    public PlayerMoveState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.movement.CheckIfShouldFlip(inputX);
        core.movement.SetVelocityX(playerData.movementVelocity * inputX);

        if (inputX == 0f)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (crouchInput)
        {
            if (inputX != 0f)
            {
                stateMachine.ChangeState(player.crouchMoveState);
            }
            else
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion
}