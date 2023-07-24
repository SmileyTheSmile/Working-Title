using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Player Condition Table", menuName = "Player Condition Table")]
public class PlayerConditionTable : ConditionTable
{
    [SerializeField] private PlayerData _playerData;

    public bool IsGrounded;
    public bool IsReloading;
    public bool IsTouchingCeiling;
    public bool IsTouchingWall;
    public bool IsTouchingLedgeHorizontal;
    public bool IsJumping;
    public bool HasStoppedFalling;
    public bool CanAttack;
    public bool IsMovingUp => NormalizedInputY > 0;
    public bool IsMovingDown => NormalizedInputY < 0;
    public bool IsMovingX => NormalizedInputX != 0;
    public bool IsMovingInCorrectDir => NormalizedInputX == MovementDir;
    public bool CanJump => NumberOfJumpsLeft > 0;
    public bool CanCrouch => NumberOfCrouchesLeft > 0;
    
    public bool IsPressingJump;
    public bool IsPressingCrouch;
    public bool IsJumpCanceled;
    public bool IsPressingPause;
    public bool IsPressingGrab;
    public bool IsPressingPrimaryAttack;
    public bool IsPressingSecondaryAttack;
    public bool IsInCoyoteTime;
    
    public int NormalizedInputX;
    public int NormalizedInputY;
    public int WeaponSwitchInput;
    public int MovementDir;
    public int FacingDir;
    public int NumberOfJumpsLeft;
    public int NumberOfCrouchesLeft;

    public Vector3 MousePosition;

    public float JumpInputStartTime;
    public float CoyoteTimeStartTime;
    public float LastStepTime;
}
