using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch In Air State", menuName = "States/Player/In Air/Crouch In Air State")]

public class PlayerCrouchInAirState : PlayerInAirState
{
    [SerializeField] protected PlayerInAirState inAirState;

    public override void Enter()
    {
        base.Enter();

        conditionManager.DecreaseAmountOfCrouchesLeft();

        conditionManager.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    public override void Exit()
    {
        base.Exit();

        conditionManager.UnCrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
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
