using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch In Air State", menuName = "States/Player/In Air/Crouch In Air State")]

public class PlayerCrouchInAirState : PlayerInAirState
{
    protected PlayerInAirState inAirState => conditionManager.inAirState;

    public override void Enter()
    {
        base.Enter();

        conditionManager.DecreaseAmountOfCrouchesLeft();

        movement.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();

        movement.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override GenericState DoTransitions()
    {
        var parentResult = base.DoTransitions();

        if (parentResult != null)
        {
            return parentResult;
        }

        if (!_isPressingCrouch && !_isGrounded)
        {
            return inAirState;
        }

        return null;
    }
}
