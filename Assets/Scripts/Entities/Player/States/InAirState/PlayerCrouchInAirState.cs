using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch In Air State", menuName = "States/Player/In Air/Crouch In Air State")]

public class PlayerCrouchInAirState : PlayerInAirState
{
    [SerializeField] protected PlayerInAirState inAirState;

    public override void Enter()
    {
        base.Enter();

        _player.CrouchInAir();
    }

    public override void Exit()
    {
        base.Exit();

        _player.UnCrouch();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (!_stats.IsPressingCrouch && !_stats.IsGrounded)
        {
            return inAirState;
        }

        return null;
    }
}
