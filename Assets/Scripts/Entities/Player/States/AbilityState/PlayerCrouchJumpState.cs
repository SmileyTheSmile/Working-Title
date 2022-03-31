using UnityEngine;

public class PlayerCrouchJumpState : PlayerJumpState
{
    public PlayerCrouchJumpState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
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

        if (!crouchInput && core.movement.crouchingForm == PlayerCrouchingForm.crouchingDown)
        {
            core.UnSquashColliderDown(playerData.standColliderHeight, playerData.crouchColliderHeight);

            core.movement.crouchingForm = PlayerCrouchingForm.normal;
        }
    }
}
