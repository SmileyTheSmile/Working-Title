using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Player Condition Table", menuName = "Player Condition Table")]
public class PlayerStats : StatsTable
{
    public bool IsGrounded;
    public bool IsReloading;
    public bool IsTouchingCeiling;
    public bool IsTouchingWall;
    public bool IsTouchingLedgeHorizontal;
    public bool IsJumping;
    public bool HasStoppedFalling;
    public bool CanAttack;
    public bool IsCrouchingDown;
    public bool IsCrouchingUp;
    public bool IsInCoyoteTime;
    
    public bool IsPressingJump;
    public bool IsPressingCrouch;
    public bool IsJumpCanceled;
    public bool IsPressingPause;
    public bool IsPressingGrab;
    public bool IsPressingPrimaryAttack;
    public bool IsPressingSecondaryAttack;
    public int NormalizedInputX;
    public int NormalizedInputY;
    public int WeaponSwitchInput;
    public Vector3 RawMousePosition;
    
    public int MovementDir = 1;
    public int FacingDir = 1;
    public int NumberOfJumpsLeft;
    public int NumberOfInAirCrouchesLeft;

    public Vector3 MousePosition;

    public float JumpInputStartTime;
    public float CoyoteTimeStartTime;
    public float LastStepTime;
    
    public bool IsMovingUp => NormalizedInputY > 0;
    public bool IsMovingDown => NormalizedInputY < 0;
    public bool IsMovingX => NormalizedInputX != 0;
    public bool IsMovingInCorrectDir => NormalizedInputX == MovementDir;
    public bool CanJump => NumberOfJumpsLeft > 0;
    public bool CanCrouch => NumberOfInAirCrouchesLeft > 0;
}
