using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Player Condition Table", menuName = "Player Condition Table")]
public class PlayerConditionTable : ConditionTable
{
    public bool IsGrounded;
    public bool IsMovingX;
    public bool IsMovingUp;
    public bool IsMovingDown;
    public bool IsReloading;
    public bool HasStoppedFalling;
    public bool CanCrouch;
    public bool CanJump;
    public bool CanAttack;
    public bool IsMovingInCorrectDir;
    public bool IsTouchingCeiling;
    public bool IsTouchingWall;
    public bool IsTouchingLedgeHorizontal;
    public bool IsJumping;
    

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
    public int MovementDir;
    public Vector3 MousePosition;
    
}
