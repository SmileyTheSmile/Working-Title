using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Jump State", menuName = "States/Player/Ability/Crouch Jump State")]

public class PlayerCrouchJumpState : PlayerJumpState
{
    public override void Enter()
    {
        base.Enter();

        _player.Crouch();
    }

    public override void Exit()
    {
        base.Exit();

        _player.UnCrouch();
    }
}
