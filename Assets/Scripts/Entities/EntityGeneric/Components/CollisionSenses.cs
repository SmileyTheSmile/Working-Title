using UnityEngine;

public class CollisionSenses : CoreComponent
{
    private Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float groundCheckHeight = 0.2f;
    [SerializeField] private float groundCheckWidth = 0.5f;
    [SerializeField] private float groundWidthOffset = 0.05f;
    [SerializeField] private float ceilingCheckHeight = 0.2f;
    [SerializeField] private float ceilingCheckWidth = 0.5f;
    [SerializeField] private float ceilingWidthOffset = 0.5f;
    [SerializeField] private float wallCheckDistance = 0.6f;

    private float halfCeilingCheckWidth;
    private float halfCeilingCheckHeight;
    private float halfGroundCheckWidth;
    private float halfGroundCheckHeight;

    //Unity Start
    private void Start()
    {
        groundCheckWidth = movement.defaultSize.x - groundWidthOffset;
        ceilingCheckWidth = movement.defaultSize.y - ceilingWidthOffset;

        halfCeilingCheckWidth = groundCheckWidth / 2;
        halfCeilingCheckHeight = groundCheckHeight / 2;
        halfGroundCheckWidth = groundCheckWidth / 2;
        halfGroundCheckHeight = groundCheckHeight / 2;
    }

    //Check if entity is grounded
    public bool Ground 
    {
        get => (Physics2D.OverlapBox(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0f, whatIsGround));
    }

    //Check if entity is touching ceiling
    public bool Ceiling 
    {
        get => Physics2D.OverlapBox(ceilingCheck.position, new Vector2(ceilingCheckWidth, ceilingCheckHeight), 0f, whatIsGround);
    }

    //Check if entity is touching a wall in front of it
    public bool WallFront  
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * movement.facingDirection, wallCheckDistance, whatIsGround);
    }

    //Check if entity is touching a wall at its back
    public bool WallBack 
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * -movement.facingDirection, wallCheckDistance, whatIsGround);
    }

    //Check if entity is nearing a ledge when wall climbing
    public bool LedgeHorizontal 
    {
        get => Physics2D.Raycast(ledgeCheckHorizontal.position, Vector2.right * movement.facingDirection, wallCheckDistance, whatIsGround);
    }

    //Check if entity is standing on a ledge
    public bool LedgeVertical 
    {
        get => Physics2D.Raycast(ledgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);
    }

    //Move the ceiling check point position
    public void MoveCeilingCheck(float oldHeight, float newHeight, float defaultColliderHeight)
    {
        if (ceilingCheck)
        {
            ceilingCheck.transform.position += Vector3.up * ((oldHeight - newHeight) * defaultColliderHeight);
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
        if (groundCheck)
        {
            if (Ground)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.blue;

            UnityEditor.Handles.DrawLine(new Vector2(groundCheck.position.x - halfGroundCheckWidth, groundCheck.position.y + halfGroundCheckHeight), new Vector2(groundCheck.position.x + halfGroundCheckWidth, groundCheck.position.y + halfGroundCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(groundCheck.position.x - halfGroundCheckWidth, groundCheck.position.y + halfGroundCheckHeight), new Vector2(groundCheck.position.x - halfGroundCheckWidth, groundCheck.position.y - halfGroundCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(groundCheck.position.x + halfGroundCheckWidth, groundCheck.position.y - halfGroundCheckHeight), new Vector2(groundCheck.position.x + halfGroundCheckWidth, groundCheck.position.y + halfGroundCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(groundCheck.position.x + halfGroundCheckWidth, groundCheck.position.y - halfGroundCheckHeight), new Vector2(groundCheck.position.x - halfGroundCheckWidth, groundCheck.position.y - halfGroundCheckHeight));
        }

        if (ceilingCheck)
        {
            if (Ceiling)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.blue;

            UnityEditor.Handles.DrawLine(new Vector2(ceilingCheck.position.x - halfCeilingCheckWidth, ceilingCheck.position.y + halfCeilingCheckHeight), new Vector2(ceilingCheck.position.x + halfCeilingCheckWidth, ceilingCheck.position.y + halfCeilingCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(ceilingCheck.position.x - halfCeilingCheckWidth, ceilingCheck.position.y + halfCeilingCheckHeight), new Vector2(ceilingCheck.position.x - halfCeilingCheckWidth, ceilingCheck.position.y - halfCeilingCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(ceilingCheck.position.x + halfCeilingCheckWidth, ceilingCheck.position.y - halfCeilingCheckHeight), new Vector2(ceilingCheck.position.x + halfCeilingCheckWidth, ceilingCheck.position.y + halfCeilingCheckHeight));
            UnityEditor.Handles.DrawLine(new Vector2(ceilingCheck.position.x + halfCeilingCheckWidth, ceilingCheck.position.y - halfCeilingCheckHeight), new Vector2(ceilingCheck.position.x - halfCeilingCheckWidth, ceilingCheck.position.y - halfCeilingCheckHeight));
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