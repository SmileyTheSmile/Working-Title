using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Jump State", menuName = "States/Player/Ability/Wall Jump State")]

public class PlayerWallJumpState : PlayerAbilityState
{
    private Vector2 _wallJumpDirection;
    private int _wallJumpMovementDirection;

    private bool _isTouchingWall => conditionManager.IsTouchingWallFrontSO.value;
    private int _movementDir => conditionManager._movementDirSO.value;

    public override void Enter()
    {
        base.Enter();
        
        inputHandler.UseJumpInput();
        conditionManager.ResetAmountOfJumpsLeft();
        conditionManager.DecreaseAmountOfJumpsLeft();

        _wallJumpMovementDirection = (_isTouchingWall ? -1 : 1) * _movementDir;
        _wallJumpDirection = (Vector2)(Quaternion.Euler(0, 0, _playerData.wallJumpAngle) * Vector2.right); //Temporary

        movement.SetVelocity(_playerData.wallJumpVelocity, _wallJumpDirection, _wallJumpMovementDirection);
        movement.CheckMovementDirection(_wallJumpMovementDirection);

        _isAbilityDone = true;
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
