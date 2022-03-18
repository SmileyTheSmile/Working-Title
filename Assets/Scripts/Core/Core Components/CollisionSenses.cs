using UnityEngine;

public class CollisionSenses : CoreComponent
{
    #region Public Wrapper Variables

    public Transform groundCheck
    {
        get
        {
            if (_groundCheck)
                return _groundCheck;

            Debug.LogError("No Ground Check on " + core.transform.parent.name);
            return null;
        }

        private set => _groundCheck = value;
    }
    public Transform ceilingCheck
    {
        get
        {
            if (_ceilingCheck)
                return _ceilingCheck;

            Debug.LogError("No Ceiling Check on " + core.transform.parent.name);
            return null;
        }

        private set => _ceilingCheck = value;
    }
    public Transform wallCheck
    {
        get
        {
            if (_wallCheck)
                return _wallCheck;

            Debug.LogError("No Wall Check on " + core.transform.parent.name);
            return null;
        }

        private set => _wallCheck = value;
    }
    public Transform ledgeCheckHorizontal
    {
        get
        {
            if (_ledgeCheckHorizontal)
                return _ledgeCheckHorizontal;

            Debug.LogError("No Horizontal Ledge Check on " + core.transform.parent.name);
            return null;
        }

        private set => _ledgeCheckHorizontal = value;
    }
    public Transform ledgeCheckVertical
    {
        get
        {
            if (_ledgeCheckVertical)
                return _ledgeCheckVertical;

            Debug.LogError("No Vertical Ledge Check on " + core.transform.parent.name);
            return null;
        }

        private set => _ledgeCheckVertical = value;
    }

    public LayerMask whatIsGround { get => _whatIsGround; set => _whatIsGround = value; }

    public float groundCheckRadius { get => _groundCheckRadius; set => _groundCheckRadius = value; }
    public float wallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }

    #endregion

    #region Private Variables

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheckHorizontal;
    [SerializeField] private Transform _ledgeCheckVertical;

    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private float _groundCheckRadius = 0.45f;
    [SerializeField] private float _wallCheckDistance = 0.5f;

    #endregion

    #region Check Functions

    public bool Ground //Check if entity is grounded
    {
        get => Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);
    }

    public bool Ceiling //Check if entity is touching ceiling
    {
        get => Physics2D.OverlapCircle(_ceilingCheck.position, _groundCheckRadius, _whatIsGround);
    }

    public bool WallFront  //Check if entity is touching a wall in front of it
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * core.movement.facingDirection, _wallCheckDistance, _whatIsGround);
    }

    public bool WallBack //Check if entity is touching a wall at its back
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * -core.movement.facingDirection, _wallCheckDistance, _whatIsGround);
    }

    public bool LedgeHorizontal //Check if entity is standing on a ledge
    {
        get => Physics2D.Raycast(_ledgeCheckHorizontal.position, Vector2.right * core.movement.facingDirection, _wallCheckDistance, _whatIsGround);
    }

    public bool LedgeVertical //Check if entity is nearing a ledge when wall climbing
    {
        get => Physics2D.Raycast(_ledgeCheckVertical.position, Vector2.down, _wallCheckDistance, _whatIsGround);
    }

    #endregion

    #region Editor Functions

    public void LogCurrentCollisions() //Debug all check values
    {
        Debug.Log($"Ground = {Ground.ToString()}, Ceiling = {Ceiling.ToString()}, Ledge = {LedgeHorizontal.ToString()}, Wall Front = {WallFront.ToString()}, Wall Back = {WallBack.ToString()}");
    }

    void OnDrawGizmos() //Draw check gizmos
    {
        if (groundCheck)
        {
            if (Ground)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(_groundCheck.position, Vector3.forward, _groundCheckRadius, 1.5f);
        }

        if (ceilingCheck)
        {
            if (Ceiling)
                UnityEditor.Handles.color = Color.green;
            else
                UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(_ceilingCheck.position, Vector3.forward, _groundCheckRadius, 1.5f);
        }

        //Gizmos.DrawLine(core.collisionSenses.wallCheck.position, core.collisionSenses.wallCheck.position + (Vector3)(Vector2.right * core.movement.facingDirection * wallCheckDistance));
        //Gizmos.DrawLine(core.collisionSenses.ledgeCheckVertical.position, core.collisionSenses.ledgeCheckVertical.position + (Vector3)(Vector2.down * wallCheckDistance));

        //UnityEditor.Handles.DrawLine(_wallCheck.position, Vector2.right * core.movement.facingDirection * _wallCheckDistance);
    }

    #endregion
}