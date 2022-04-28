using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private Vector2 dashDirectionInput;
    private Vector2 dashDirection;

    private float lastDashTime;

    private bool dashInputStop;
    private bool canDash;
    private bool isHolding;

    public PlayerDashState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        isHolding = true;
        canDash = false;

        player.inputHandler.UseDashInput();
        dashDirection = Vector2.right * movement.facingDirection;

        Time.timeScale = playerData.holdTimeScale;
        startTime = Time.unscaledTime;
    }

    public override void Exit()
    {
        base.Exit();


        if (movement.currentVelocity.y > 0)
        {
            movement?.SetVelocityY(movement.currentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        player.animator.SetFloat("velocityY", movement.currentVelocity.y);
        player.animator.SetFloat("velocityX", Mathf.Abs(movement.currentVelocity.x));

        if (isHolding)
        {
            dashDirectionInput = player.inputHandler.mousePositionInput - player.transform.position;
            dashInputStop = player.inputHandler.dashInputStop;

            if (dashDirectionInput != Vector2.zero)
            {
                dashDirection = dashDirectionInput;
                dashDirection.Normalize();
            }

            float angle = Vector2.SignedAngle(Vector2.right, dashDirection);

            if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
            {
                isHolding = false;
                Time.timeScale = 1f;
                startTime = Time.time;

                movement?.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                movement?.SetVelocity(playerData.dashVelocity, dashDirection);

                movement.rigidBody.drag = playerData.drag;
            }
        }
        else
        {
            movement?.SetVelocity(playerData.dashVelocity, dashDirection);

            if (Time.time >= startTime + playerData.dashTime)
            {
                movement.rigidBody.drag = 0f;
                isAbilityDone = true;
                lastDashTime = Time.time;
                Debug.Log("sdfsd");
            }
        }
    }

    public bool CheckIfCanDash()
    {
        return canDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash() => canDash = true;
}
