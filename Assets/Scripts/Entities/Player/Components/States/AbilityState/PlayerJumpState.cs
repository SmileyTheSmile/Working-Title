using UnityEngine;

[CreateAssetMenu(fileName = "Player Jump State", menuName = "States/Player/Ability/Jump State")]

public class PlayerJumpState : PlayerAbilityState
{
    private int _amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData)
    {
        _amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        inputHandler?.UseJumpInput();
        movement?.SetVelocityY(_playerData.jumpVelocity);

        _player.inAirState.SetIsJumping();
        _player.crouchInAirState.SetIsJumping();

        DecreaseAmountOfJumpsLeft();
        _isAbilityDone = true;
    }

    public bool CanJump()
    {
        if (_amountOfJumpsLeft > 0)
        {
            return true;
        }

        return false;
    }

    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
}