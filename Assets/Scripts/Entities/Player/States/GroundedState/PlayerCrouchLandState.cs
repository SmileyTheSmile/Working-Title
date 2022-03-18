using UnityEngine;

public class PlayerCrouchLandState : PlayerLandState
{
    public PlayerCrouchLandState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) { }
}
