using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    #region State Functions

    public PlayerIdleState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        core.movement.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0f)
        {
            if (crouchInput)
            {
                core.SquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

                stateMachine.ChangeState(player.crouchMoveState);
            }
            else
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
        else if (crouchInput)
        {
            core.SquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

            stateMachine.ChangeState(player.crouchIdleState);
        }
    }

    #endregion
}
