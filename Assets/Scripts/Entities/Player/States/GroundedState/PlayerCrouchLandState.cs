using UnityEngine;

[CreateAssetMenu(fileName = "Player Crouch Land State", menuName = "States/Player/Grounded/Crouch Land State")]

public class PlayerCrouchLandState : PlayerGroundedState
{
    protected PlayerCrouchMoveState crouchMoveState => conditionManager.crouchMoveState;
    protected PlayerMoveState moveState => conditionManager.moveState;
    protected PlayerCrouchIdleState crouchIdleState => conditionManager.crouchIdleState;
    protected PlayerIdleState idleState => conditionManager.idleState;

    protected AudioSourcePlayer _fallSound => conditionManager.fallSound;

    public override void Enter()
    {
        base.Enter();

        Step();

        movement.CrouchDown(_playerData.standColliderHeight, _playerData.crouchColliderHeight, _isPressingCrouch);
    }

    protected virtual void Step()
    {
        if (_fallSound)
            _fallSound.Play();
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
