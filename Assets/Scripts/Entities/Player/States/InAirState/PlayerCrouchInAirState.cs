using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch In Air State", menuName = "States/Player/In Air/Crouch In Air State")]

public class PlayerCrouchInAirState : PlayerInAirState
{
    [SerializeField] protected PlayerInAirState inAirState;

    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.CrouchInAir();
    }

    public override void Exit()
    {
        base.Exit();

        _temporaryComponent.UnCrouch();
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();
        if (parentResult)
            return parentResult;

        if (!_conditions.IsPressingCrouch && !_conditions.IsGrounded)
        {
            return inAirState;
        }

        return null;
    }
}
