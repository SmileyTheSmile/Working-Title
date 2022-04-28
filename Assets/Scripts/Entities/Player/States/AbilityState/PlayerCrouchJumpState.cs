using UnityEngine;

public class PlayerCrouchJumpState : PlayerJumpState
{
    public PlayerCrouchJumpState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        crouchInput = player.inputHandler.crouchInput;

        movement?.CrouchDown(playerData.standColliderHeight, playerData.crouchColliderHeight, crouchInput);
    }

    public override void Exit()
    {
        base.Exit();

        crouchInput = player.inputHandler.crouchInput;
        
        movement?.UnCrouchDown(playerData.standColliderHeight, playerData.crouchColliderHeight, crouchInput);
    }
}
