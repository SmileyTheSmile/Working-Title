using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch In Air State", menuName = "States/Player/In Air/Crouch In Air State")]

public class PlayerCrouchInAirState : PlayerInAirState
{
    private int _amountOfCrouchesLeft;

    public PlayerCrouchInAirState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName)
    {
        _amountOfCrouchesLeft = playerData.amountOfCrouches;
    }

    public override void Enter()
    {
        base.Enter();

        _crouchInput = inputHandler.crouchInput;

        DecreaseAmountOfCrouchesLeft();

        movement?.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }

    public override void Exit()
    {
        base.Exit();

        _crouchInput = inputHandler.crouchInput;

        movement?.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _crouchInput);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (!_crouchInput && !_isGrounded)
        {
            stateMachine?.ChangeState(_player.inAirState);
        }
    }

    public bool CanCrouch()
    {
        if (_amountOfCrouchesLeft > 0)
        {
            return true;
        }

        return false;
    }

    public void ResetAmountOfCrouchesLeft() => _amountOfCrouchesLeft = _playerData.amountOfCrouches;

    public void DecreaseAmountOfCrouchesLeft() => _amountOfCrouchesLeft--;
}
