using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Jump State", menuName = "States/Player/Ability/Crouch Jump State")]

public class PlayerCrouchJumpState : PlayerJumpState
{
    public PlayerCrouchJumpState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        _crouchInput = inputHandler.crouchInput;

        movement?.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }

    public override void Exit()
    {
        base.Exit();

        _crouchInput = inputHandler.crouchInput;
        
        movement?.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }
}
