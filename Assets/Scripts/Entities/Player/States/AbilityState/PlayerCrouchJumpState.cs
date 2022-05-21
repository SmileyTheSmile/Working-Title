using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Jump State", menuName = "States/Player/Ability/Crouch Jump State")]

public class PlayerCrouchJumpState : PlayerJumpState
{
    public override void Enter()
    {
        base.Enter();

        movement.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();
        
        movement.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }
}
