using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch In Air State", menuName = "States/Player/In Air/Crouch In Air State")]

public class PlayerCrouchInAirState : PlayerInAirState
{
    public PlayerCrouchInAirState(Player player, PlayerData playerData, string animBoolName)
    : base(player, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        inputHandler?.DecreaseAmountOfCrouchesLeft();

        movement?.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();

        movement?.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void DoTransitions()
    {
        base.DoTransitions();

        if (!_isPressingCrouch && !_isGrounded)
        {
            stateMachine?.ChangeState(_player.inAirState);
        }
    }
}
