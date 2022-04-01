using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    #region Input Variables

    public Vector2 dashDirectionInput;
    public bool dashInputStop;

    #endregion

    #region Check Variables

    public bool canDash;
    public bool isHolding;

    #endregion

    #region Utility Variables

    public float lastDashTime;
    public Vector2 dashDirection;

    #endregion

    #region State Functions

    public PlayerDashState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        isHolding = true;
        canDash = false;

        player.inputHandler.UseDashInput();
        dashDirection = Vector2.right * core.movement.facingDirection;

        core.movement.dashDirectionIndicator.gameObject.SetActive(true);
        Time.timeScale = playerData.holdTimeScale;
        startTime = Time.unscaledTime;
    }

    public override void Exit()
    {
        base.Exit();

        core.movement.dashDirectionIndicator.gameObject.SetActive(false);

        if (core.movement.currentVelocity.y > 0)
        {
            core.movement.SetVelocityY(core.movement.currentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        player.animator.SetFloat("velocityY", core.movement.currentVelocity.y);
        player.animator.SetFloat("velocityX", Mathf.Abs(core.movement.currentVelocity.x));

        if (isHolding)
        {
            dashDirectionInput = player.inputHandler.mouseDirectionInput;
            dashInputStop = player.inputHandler.dashInputStop;

            if (dashDirectionInput != Vector2.zero)
            {
                dashDirection = dashDirectionInput;
                dashDirection.Normalize();
            }

            float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
            core.movement.dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle);

            if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
            {
                isHolding = false;
                Time.timeScale = 1f;
                startTime = Time.time;

                core.movement.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                core.movement.rigidBody.drag = playerData.drag;
                core.movement.SetVelocity(playerData.dashVelocity, dashDirection);
            }
        }
        else
        {
            core.movement.SetVelocity(playerData.dashVelocity, dashDirection);

            if (Time.time >= startTime + playerData.dashTime)
            {
                core.movement.rigidBody.drag = 0f;
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #endregion

    #region Utility Functions

    public bool CheckIfCanDash()
    {
        return canDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash() => canDash = true;

    #endregion
}