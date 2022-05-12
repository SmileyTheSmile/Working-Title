using UnityEngine;

public class CollisionSenses : CoreComponent
{

    //Check if entity is grounded
    public StateCollisionCheckCondition _groundCheck;
    //Check if entity is touching ceiling
    public StateCollisionCheckCondition _ceilingCheck;
    //Check if entity is touching a wall in front of it
    public StateCollisionCheckCondition _wallFrontCheck;
    //Check if entity is touching a wall at its back
    public StateCollisionCheckCondition _wallBackCheck;
    //Check if entity is nearing a ledge when wall climbing
    public StateCollisionCheckCondition _ledgeHorizontalCheck;
    //Check if entity is standing on a ledge
    public StateCollisionCheckCondition _ledgeCheckVertical;

    //Debug all check values
    public override void LogComponentInfo()
    {
        Debug.Log($"Ground = {_groundCheck.value}\nCeiling = {_ceilingCheck.value}\nWall Front = {_wallFrontCheck.value}\nWall Back = {_wallBackCheck.value}Ledge Horizontal = {_ledgeHorizontalCheck.value}\n");
    }
}