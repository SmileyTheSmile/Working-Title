using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Land State", menuName = "States/Player/Grounded/Crouch Land State")]

public class PlayerCrouchLandState : PlayerGroundedState
{
    [SerializeField] protected PlayerCrouchMoveState crouchMoveState;
    [SerializeField] protected PlayerMoveState moveState;
    [SerializeField] protected PlayerCrouchIdleState crouchIdleState;
    [SerializeField] protected PlayerIdleState idleState;

    protected AudioSourcePlayer _fallSound => conditionManager.fallSound;

    public override void Enter()
    {
        base.Enter();

        if (_fallSound)
            _fallSound.Play();

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

        if (_isMovingX)
        {
            if (_isPressingCrouch)
            {
                return crouchMoveState;
            }
            else
            {
                return moveState;
            }
        }
        else
        {
            if (_isAnimationFinished)
            {
                if (_isPressingCrouch)
                {
                    return crouchIdleState;
                }
                else
                {
                    return idleState;
                }
            }
        }

        return null;
    }
}
