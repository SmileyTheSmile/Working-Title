using UnityEngine;

[CreateAssetMenu(fileName = "Player Jump State", menuName = "States/Player/Ability/Jump State")]

public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        inputHandler?.UseJumpInput();
        movement?.SetVelocityY(_playerData.jumpVelocity);

        _player.inAirState.SetIsJumping();
        _player.crouchInAirState.SetIsJumping();

        inputHandler?.DecreaseAmountOfJumpsLeft();
        _isAbilityDone = true;
    }
}