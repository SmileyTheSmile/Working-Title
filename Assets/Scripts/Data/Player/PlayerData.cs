using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]

public class PlayerData : ScriptableObject
{
    #region Move State

    [Header("Move State")]
    public float movementVelocity = 10f;

    #endregion

    #region Jump State

    [Header("Jump State")]
    public float jumpVelocity = 10f;
    public int amountOfJumps = 2;

    #endregion

    #region In Air State

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float airControlPercentage = 0.5f;

    #endregion

    #region Wall Climb State

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3f;

    #endregion

    #region Wall Slide State

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;

    #endregion

    #region Wall Jump State

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 15f;
    public float wallJumpTime = 0.6f;
    public float wallJumpAngle = 45f;

    #endregion

    #region Ledge Climb State

    [Header("Ledge Climb State")]
    public float ledgeClimbVelocity = 5f;
    public float ledgeCheckDisabledTime = 0.2f;

    #endregion

    #region Dash State

    [Header("Dash State")]
    public float dashCooldown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.1f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;

    #endregion

    #region Crouch States

    [Header("Crouch States")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 0.5f;
    public float standColliderHeight = 1f;
    public int amountOfCrouches = 2;

    #endregion

    #region Utility Variables

    public LayerMask whatIsGround;

    #endregion
}
