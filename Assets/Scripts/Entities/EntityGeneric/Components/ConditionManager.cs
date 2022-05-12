using UnityEngine;

public class ConditionManager : CoreComponent
{
    //Check if entity is grounded
    public CollisionCheckTransitionCondition IsGroundedSO;
    //Check if entity is touching ceiling
    public CollisionCheckTransitionCondition IsTouchingCeilingSO;
    //Check if entity is touching a wall in front of it
    public CollisionCheckTransitionCondition IsTouchingWallFrontSO;
    //Check if entity is touching a wall at its back
    public CollisionCheckTransitionCondition IsTouchingWallBackSO;
    //Check if entity is nearing a ledge when wall climbing
    public CollisionCheckTransitionCondition IsTouchingLedgeHorizontalSO;
    //Check if entity is standing on a ledge
    public CollisionCheckTransitionCondition IsTouchingLedgeVerticalSO;

    public InputTransitionCondition IsPressingJumpSO;
    public InputTransitionCondition IsJumpCanceledSO;
    public InputTransitionCondition IsPressingGrabSO;
    public InputTransitionCondition IsPressingCrouchSO;
    public InputTransitionCondition IsPressingPrimaryAttackSO;
    public InputTransitionCondition IsPressingSecondaryAttackSO;
    public InputTransitionCondition IsMovingSO;
}