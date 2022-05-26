using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Jump State", menuName = "States/Player/Ability/Wall Jump State")]

public class PlayerWallJumpState : PlayerAbilityState
{
    [SerializeField] private CollisionCheckTransitionCondition IsTouchingWallFrontSO;
    [SerializeField] private ScriptableInt _movementDirSO;

    private bool _isTouchingWall => IsTouchingWallFrontSO.value;
    private int _movementDir => _movementDirSO.value;

    public override void Enter()
    {
        base.Enter();
        
        inputHandler.UseJumpInput();
        conditionManager.ResetAmountOfJumpsLeft();
        conditionManager.DecreaseAmountOfJumpsLeft();

        int wallJumpMovementDirection = (_isTouchingWall ? -1 : 1) * _movementDir;
        Vector2 wallJumpDirection = (Vector2)(Quaternion.Euler(0, 0, _playerData.wallJumpAngle) * Vector2.right); //Temporary

        movement.SetVelocity(_playerData.wallJumpVelocity, wallJumpDirection, wallJumpMovementDirection);
        movement.CheckMovementDirection(wallJumpMovementDirection);
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
