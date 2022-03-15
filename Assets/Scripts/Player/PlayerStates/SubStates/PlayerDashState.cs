using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool canDash { get; private set; }
    public bool isHolding;
    public bool dashInputStop;

    public float lastDashTime;

    public Vector2 dashDirection;
    public Vector2 dashDirectionInput;
    public Vector2 lastAIPos;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
    }

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
        dashDirection = Vector2.right * player.facingDirection;

        player.dashDirectionIndicator.gameObject.SetActive(true);
        Time.timeScale = playerData.holdTimeScale;
        startTime = Time.unscaledTime;
    }

    public override void Exit()
    {
        base.Exit();

        player.dashDirectionIndicator.gameObject.SetActive(false);

        if (player.currentVelocity.y > 0)
        {
            core.movement.SetVelocityY(player.currentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState)
        {
            return;
        }

        player.animator.SetFloat("velocityY", player.currentVelocity.y);
        player.animator.SetFloat("velocityX", Mathf.Abs(player.currentVelocity.x));


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
            player.dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle);

            if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
            {
                isHolding = false;
                Time.timeScale = 1f;
                startTime = Time.time;
                player.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                player.rigidBody.drag = playerData.drag;
                core.movement.SetVelocity(playerData.dashVelocity, dashDirection);
                PlaceAfterImage();
            }
        }
        else
        {
            core.movement.SetVelocity(playerData.dashVelocity, dashDirection);
            CheckIfShouldPlaceAfterImage();

            if (Time.time >= startTime + playerData.dashTime)
            {
                player.rigidBody.drag = 0f;
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distanceBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        //Add afterimages
    }

    public bool CheckIfCanDash()
    {
        return canDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash() => canDash = true;
}
