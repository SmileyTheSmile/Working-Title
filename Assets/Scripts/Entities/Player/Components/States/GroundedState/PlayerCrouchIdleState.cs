using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public PlayerCrouchIdleState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        movement?.SetVelocityZero();

        crouchInput = inputHandler.crouchInput;

        movement?.CrouchDown(playerData.standColliderHeight, playerData.crouchColliderHeight, crouchInput);
    }

    public override void Exit()
    {
        base.Exit();

        crouchInput = inputHandler.crouchInput;

        movement?.UnCrouchDown(playerData.standColliderHeight, playerData.crouchColliderHeight, crouchInput);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputX != 0)
        {
            if (crouchInput || isTouchingCeiling)
            {
                stateMachine?.ChangeState(player.crouchMoveState);
            }
            else
            {

                stateMachine?.ChangeState(player.moveState);
            }
        }
        else if (!crouchInput && !isTouchingCeiling)
        {
            stateMachine?.ChangeState(player.idleState);
        }
    }
}