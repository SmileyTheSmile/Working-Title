using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]

public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 10f;
    public float jumpInputHoldTime = 0.2f;
    public int numberOfJumps = 2;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float defaultAirControlPercentage = 1f;
    public float airControlPercentage = 0.5f;

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 15f;
    public float wallJumpTime = 0.6f;
    public float wallJumpAngle = 45f;

    [Header("Ledge Climb State")]
    public float ledgeClimbVelocity = 5f;
    public float ledgeCheckDisabledTime = 0.2f;

    [Header("Dash State")]
    public float dashCooldown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.1f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;

    [Header("Crouch States")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 0.5f;
    public float standColliderHeight = 1f;
    public float crouchHeightDifference => standColliderHeight - crouchColliderHeight;
    public int numberOfCrouches = 2;


    public float StepDelay = 3;
    public LayerMask whatIsGround;
}
