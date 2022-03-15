using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public LayerMask whatIsGround { get => _whatIsGround; set => _whatIsGround = value; }

    public Transform groundCheck { get => _groundCheck; private set => _groundCheck = value; }
    public Transform ceilingCheck { get => _ceilingCheck; private set => _ceilingCheck = value; }
    public Transform ledgeCheck { get => _ledgeCheck; private set => _ledgeCheck = value; }
    public Transform wallCheck { get => _wallCheck; private set => _wallCheck = value; }

    public float groundCheckRadius { get => _groundCheckRadius; set => _groundCheckRadius = value; }
    public float wallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }

    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private Transform _wallCheck;


    [SerializeField] private float _groundCheckRadius = 0.45f;
    [SerializeField] private float _wallCheckDistance = 0.5f;

    #region Check Functions

    public void LogCurrentCollisions()
    {
        Debug.Log("Ground = " + Ground.ToString() + ", Ceiling = " + Ceiling.ToString() + ", Ledge = " + Ledge.ToString() + ", Wall = " + WallFront.ToString());
    }

    public bool Ground
    {
        get => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool Ceiling
    {
        get => Physics2D.OverlapCircle(ceilingCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * core.movement.facingDirection, wallCheckDistance, whatIsGround);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * core.movement.facingDirection, wallCheckDistance, whatIsGround);
    }

    public bool Ledge
    {
        get => Physics2D.Raycast(ledgeCheck.position, Vector2.right * core.movement.facingDirection, wallCheckDistance, whatIsGround);
    }

    #endregion

    void OnDrawGizmos()
    {
        if (Ground)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(groundCheck.position, Vector3.forward, groundCheckRadius, 1.5f);

        if (Ceiling)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(ceilingCheck.position, Vector3.forward, groundCheckRadius, 1.5f);

        //UnityEditor.Handles.DrawLine(wallCheck.position, Vector2.right * core.movement.facingDirection * wallCheckDistance);
    }
}
