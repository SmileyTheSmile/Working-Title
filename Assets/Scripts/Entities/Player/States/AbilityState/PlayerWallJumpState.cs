using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Jump State", menuName = "States/Player/Ability/Wall Jump State")]

public class PlayerWallJumpState : PlayerAbilityState
{
    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.UseJumpInput();
        _temporaryComponent.ResetAmountOfJumpsLeft();
        _temporaryComponent.DecreaseAmountOfJumpsLeft();

        int wallJumpMovementDirection = (_conditions.IsTouchingWall ? -1 : 1) * _conditions.MovementDir;
        Vector2 wallJumpDirection = (Vector2)(Quaternion.Euler(0, 0, _playerData.wallJumpAngle) * Vector2.right); //Temporary

        _movement.SetVelocityAtAngle(_playerData.wallJumpVelocity, wallJumpDirection, wallJumpMovementDirection);
        _temporaryComponent.CheckMovementDirection(wallJumpMovementDirection);
    }

    /*
        public override void DoActions()
        {
            base.DoActions();

            player.animator.SetFloat("velocityX", core.movement.currentVelocity.y);
            player.animator.SetFloat("velocityY", Mathf.Abs(core.movement.currentVelocity.x));

            if (Time.time >= startTime + playerData.wallJumpTime)
            {
                isAbilityDone = true;
            }
        }
    */
}
