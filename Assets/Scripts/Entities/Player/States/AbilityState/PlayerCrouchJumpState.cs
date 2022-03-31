using UnityEngine;

public class PlayerCrouchJumpState : PlayerJumpState
{
    public PlayerCrouchJumpState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }
}
