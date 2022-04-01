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

    public float wallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }

    #endregion

    #region Private Variables

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheckHorizontal;
    [SerializeField] private Transform _ledgeCheckVertical;

    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private float _groundCheckHeight = 0.2f;
    [SerializeField] private float _groundCheckWidth = 0.5f;
    [SerializeField] private float _groundWidthOffset = 0f;
    [SerializeField] private float _ceilingCheckHeight = 0.2f;
    [SerializeField] private float _ceilingCheckWidth = 0.5f;
    [SerializeField] private float _ceilingWidthOffset = 0.01f;
    [SerializeField] private float _wallCheckDistance = 0.5f;

    private float _halfCeilingCheckWidth;
    private float _halfCeilingCheckHeight;
    private float _halfGroundCheckWidth;
    private float _halfGroundCheckHeight;

    #endregion

    #region Unity Functions

    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponentInParent<BoxCollider2D>();

        _groundCheckWidth = _collider.size.x - _groundWidthOffset;
        _ceilingCheckWidth = _collider.size.y - _ceilingWidthOffset;

        _halfCeilingCheckWidth = _groundCheckWidth / 2;
        _halfCeilingCheckHeight = _groundCheckHeight / 2;
        _halfGroundCheckWidth = _groundCheckWidth / 2;
        _halfGroundCheckHeight = _groundCheckHeight / 2;
    }

    public void DisableLedgeCheck()
    {
        ledgeCheckHorizontal.gameObject.SetActive(false);
    }

    public void EnableLedgeCheck()
    {
        ledgeCheckHorizontal.gameObject.SetActive(true);
    }

    #endregion

    #region Check Functions

    public bool Ground //Check if entity is grounded
    {
        get => Physics2D.OverlapBox(_groundCheck.position, new Vector2(_groundCheckWidth, _groundCheckHeight), 0f, _whatIsGround);
    }

    public bool Ceiling //Check if entity is touching ceiling
    {
        get => Physics2D.OverlapBox(_ceilingCheck.position, new Vector2(_ceilingCheckWidth, _ceilingCheckHeight), 0f, _whatIsGround);
    }

    public bool WallFront  //Check if entity is touching a wall in front of it
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * core.movement.facingDirection, _wallCheckDistance, _whatIsGround);
    }

    public bool WallBack //Check if entity is touching a wall at its back
    {
        get => Physics2D.Raycast(_wallCheck.position, Vector2.right * -core.movement.facingDirection, _wallCheckDistance, _whatIsGround);
    }

    public bool LedgeHorizontal //Check if entity is nearing a ledge when wall climbing
    {
        get => Physics2D.Raycast(_ledgeCheckHorizontal.position, Vector2.right * core.movement.facingDirection, _wallCheckDistance, _whatIsGround);
    }

    public bool LedgeVertical //Check if entity is standing on a ledge
    {
        get => Physics2D.Raycast(_ledgeCheckVertical.position, Vector2.down, _wallCheckDistance, _whatIsGround);
    }

    #endregion

    #region Editor Functions

    public void LogCurrentCollisions() //Debug all check values
    {
        Debug.Log($"Ground = {Ground.ToString()}, Ceiling = {Ceiling.ToString()}, Ledge = {LedgeHorizontal.ToString()}, Wall Front = {WallFront.ToString()}, Wall Back = {WallBack.ToString()}");
    }

    void OnDrawGizmosSelected() //Draw check gizmos
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

        if (_ledgeCheckHorizontal)
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
        }
    }

    #endregion
}