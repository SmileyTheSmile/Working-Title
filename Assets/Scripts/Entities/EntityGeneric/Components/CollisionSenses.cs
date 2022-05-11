using UnityEngine;

public class CollisionSenses : CoreComponent
{
    [SerializeField] private StateCollisionCheckCondition _groundCheck;
    [SerializeField] private StateCollisionCheckCondition _ceilingCheck;
    [SerializeField] private StateCollisionCheckCondition _wallFront;
    [SerializeField] private StateCollisionCheckCondition _wallBack;
    [SerializeField] private StateCollisionCheckCondition _ledgeCheckHorizontal;
    [SerializeField] private StateCollisionCheckCondition _ledgeCheckVertical;

    //Check if entity is grounded
    public bool Ground 
    {
        get => _groundCheck.value;
    }

    //Check if entity is touching ceiling
    public bool Ceiling 
    {
        get => _ceilingCheck.value;
    }

    //Check if entity is touching a wall in front of it
    public bool WallFront  
    {
        get => _wallFront.value;
    }

    //Check if entity is touching a wall at its back
    public bool WallBack 
    {
        get => _wallBack.value;
    }

    //Check if entity is nearing a ledge when wall climbing
    public bool LedgeHorizontal 
    {
        get => _ledgeCheckHorizontal.value;
    }

    //Check if entity is standing on a ledge
    public bool LedgeVertical
    {
        get => _ledgeCheckVertical.value;
    }

    //Debug all check values
    public override void LogComponentInfo()
    {
        Debug.Log($"Ground = {Ground.ToString()}\nCeiling = {Ceiling.ToString()}\nLedge Horisontal = {LedgeHorizontal.ToString()}\nWall Front = {WallFront.ToString()}\nWall Back = {WallBack.ToString()}");
    }
}