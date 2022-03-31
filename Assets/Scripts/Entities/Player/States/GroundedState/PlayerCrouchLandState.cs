using UnityEngine;

public class PlayerCrouchLandState : PlayerGroundedState
{
    public PlayerCrouchLandState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        crouchInput = player.inputHandler.crouchInput;

        if (core.movement.crouchingForm == PlayerCrouchingForm.normal && crouchInput)
        {
            core.SquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

            core.movement.crouchingForm = PlayerCrouchingForm.crouchingDown;
        }
    }

    public override void Exit()
    {
        base.Exit();

        crouchInput = player.inputHandler.crouchInput;

        if (!crouchInput && core.movement.crouchingForm == PlayerCrouchingForm.crouchingDown && !isTouchingCeiling)
        {
            core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

            core.movement.crouchingForm = PlayerCrouchingForm.normal;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0)
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
        else
        {
            if (isAnimationFinished)
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
        }
    }
}
