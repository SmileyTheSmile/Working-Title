using System.ComponentModel.Design;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Movement movement
    { get => _movement ?? _core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheckHorizontal;
    [SerializeField] private Transform _ledgeCheckVertical;
    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private float _groundCheckHeight = 0.2f;
    [SerializeField] private float _groundWidthOffset = 0.05f;
    [SerializeField] private float _ceilingCheckHeight = 0.2f;
    [SerializeField] private float _ceilingWidthOffset = 0.5f;
    [SerializeField] private float _wallCheckDistance = 0.6f;

    private float _groundCheckWidth => movement.defaultSize.x - _groundWidthOffset;
    private float _ceilingCheckWidth => movement.defaultSize.y - _ceilingWidthOffset;
    private float _halfCeilingCheckWidth => _groundCheckWidth / 2;
    private float _halfCeilingCheckHeight => _groundCheckHeight / 2;
    private float _halfGroundCheckWidth => _groundCheckWidth / 2;
    private float _halfGroundCheckHeight => _groundCheckHeight / 2;

    //Check if entity is grounded
    public bool Ground 
    {
        get => (Physics2D.OverlapBox(_groundCheck.position, new Vector2(_groundCheckWidth, _groundCheckHeight), 0f, _whatIsGround));
    }

    //Check if entity is touching ceiling
    public bool Ceiling 
    {
        get => Physics2D.OverlapBox(_ceilingCheck.position, new Vector2(_ceilingCheckWidth, _ceilingCheckHeight), 0f, _whatIsGround);
    }

    //Check if entity is touching a wall in front of it
    public bool WallFront  
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * movement.movementDirection, _wallCheckDistance, _whatIsGround);
    }

    //Check if entity is touching a wall at its back
    public bool WallBack 
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * -movement.movementDirection, _wallCheckDistance, _whatIsGround);
    }

    //Check if entity is nearing a ledge when wall climbing
    public bool LedgeHorizontal 
    {
        get => Physics2D.Raycast(_ledgeCheckHorizontal.position, Vector2.right * movement.movementDirection, _wallCheckDistance, _whatIsGround);
    }

    //Check if entity is standing on a ledge
    public bool LedgeVertical 
    {
        get => Physics2D.Raycast(_ledgeCheckVertical.position, Vector2.down, _wallCheckDistance, _whatIsGround);
    }

    //Move the ceiling check point position
    public void MoveCeilingCheck(float oldHeight, float newHeight, float defaultColliderHeight)
    {
        if (_ceilingCheck)
        {
            _ceilingCheck.transform.position += Vector3.up * ((oldHeight - newHeight) * defaultColliderHeight);
        }
    }

    //Debug all check values
    public override void LogComponentInfo()
    {
        Debug.Log($"Ground = {Ground.ToString()}\nCeiling = {Ceiling.ToString()}\nLedge Horisontal = {LedgeHorizontal.ToString()}\nWall Front = {WallFront.ToString()}\nWall Back = {WallBack.ToString()}");
    }

    //Draw gizmos
    void OnDrawGizmos() 
    {
        if (_groundCheck)
        {
            if (Ground)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.blue;

            UnityEditor.Handles.DrawLine(new Vector2(_groundCheck.position.x - _halfGroundCheckWidth, _groundCheck.position.y + _halfGroundCheckHeight), new Vector2(_groundCheck.position.x + _halfGroundCheckWidth, _groundCheck.position.y + _halfGroundCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(_groundCheck.position.x - _halfGroundCheckWidth, _groundCheck.position.y + _halfGroundCheckHeight), new Vector2(_groundCheck.position.x - _halfGroundCheckWidth, _groundCheck.position.y - _halfGroundCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(_groundCheck.position.x + _halfGroundCheckWidth, _groundCheck.position.y - _halfGroundCheckHeight), new Vector2(_groundCheck.position.x + _halfGroundCheckWidth, _groundCheck.position.y + _halfGroundCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(_groundCheck.position.x + _halfGroundCheckWidth, _groundCheck.position.y - _halfGroundCheckHeight), new Vector2(_groundCheck.position.x - _halfGroundCheckWidth, _groundCheck.position.y - _halfGroundCheckHeight));
        }

        if (_ceilingCheck)
        {
            if (Ceiling)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.blue;

            UnityEditor.Handles.DrawLine(new Vector2(_ceilingCheck.position.x - _halfCeilingCheckWidth, _ceilingCheck.position.y + _halfCeilingCheckHeight), new Vector2(_ceilingCheck.position.x + _halfCeilingCheckWidth, _ceilingCheck.position.y + _halfCeilingCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(_ceilingCheck.position.x - _halfCeilingCheckWidth, _ceilingCheck.position.y + _halfCeilingCheckHeight), new Vector2(_ceilingCheck.position.x - _halfCeilingCheckWidth, _ceilingCheck.position.y - _halfCeilingCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(_ceilingCheck.position.x + _halfCeilingCheckWidth, _ceilingCheck.position.y - _halfCeilingCheckHeight), new Vector2(_ceilingCheck.position.x + _halfCeilingCheckWidth, _ceilingCheck.position.y + _halfCeilingCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(_ceilingCheck.position.x + _halfCeilingCheckWidth, _ceilingCheck.position.y - _halfCeilingCheckHeight), new Vector2(_ceilingCheck.position.x - _halfCeilingCheckWidth, _ceilingCheck.position.y - _halfCeilingCheckHeight));
        }

        /*if (_ledgeCheckHorizontal)
        {
            if (LedgeHorizontal)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.yellow;

            UnityEditor.Handles.DrawLine(_ledgeCheckHorizontal.position, Vector3.right * core.movement.facingDirection * _wallCheckDistance + _ledgeCheckHorizontal.position);
        }

        if (_wallCheck)
        {
            if (WallFront)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawLine(_wallCheck.position, Vector3.right * core.movement.facingDirection * _wallCheckDistance + _wallCheck.position);

            if (WallBack)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawLine(_wallCheck.position, Vector3.right * core.movement.facingDirection * _wallCheckDistance * -1 + _wallCheck.position);
        }*/
    }
}