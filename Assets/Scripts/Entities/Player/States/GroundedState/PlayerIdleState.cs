using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    #region State Functions

    public PlayerIdleState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0f)
        {
            if (crouchInput)
            {
                stateMachine.ChangeState(player.crouchMoveState);
            }
            else
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
        else if (crouchInput)
        {
            stateMachine.ChangeState(player.crouchIdleState);
        }
    }

    #endregion
}
