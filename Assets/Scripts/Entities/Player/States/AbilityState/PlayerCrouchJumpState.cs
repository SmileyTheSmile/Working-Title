using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Jump State", menuName = "States/Player/Ability/Crouch Jump State")]

public class PlayerCrouchJumpState : PlayerJumpState
{
    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.Crouch();
    }

    public override void Exit()
    {
        base.Exit();

        _temporaryComponent.UnCrouch();
    }
}
