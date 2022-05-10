using UnityEngine;

[CreateAssetMenu(fileName = "Player Wall Jump State", menuName = "States/Player/Ability/Wall Jump State")]

public class PlayerWallJumpState : PlayerAbilityState
{
    private Vector2 _wallJumpDirection;
    private int _wallJumpMovementDirection;

    public PlayerWallJumpState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        _wallJumpDirection = (Vector2)(Quaternion.Euler(0, 0, _playerData.wallJumpAngle) * Vector2.right); //Temporary

        inputHandler?.UseJumpInput();

        _player.jumpState.ResetAmountOfJumpsLeft();
        _player.jumpState.DecreaseAmountOfJumpsLeft();

        movement?.SetVelocity(_playerData.wallJumpVelocity, _wallJumpDirection, _wallJumpMovementDirection);
        movement?.CheckMovementDirection(_wallJumpMovementDirection);

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
    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        _wallJumpMovementDirection = (isTouchingWall ? -1 : 1) * movement.movementDirection;
    }
}
