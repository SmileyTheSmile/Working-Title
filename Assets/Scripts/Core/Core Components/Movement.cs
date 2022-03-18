using UnityEngine;

public class Movement : CoreComponent
{
    #region Movement Components

    public Rigidbody2D rigidBody
    {
        get
        {
            if (_rigidBody)
                return _rigidBody;

            Debug.LogError("No RigidBody on " + core.transform.parent.name);
            return null;
        }

        private set => _rigidBody = value;
    }
    public BoxCollider2D boxCollider
    {
        get
        {
            if (_boxCollider)
                return _boxCollider;

            Debug.LogError("No BoxCollider on " + core.transform.parent.name);
            return null;
        }

        private set => _boxCollider = value;
    }

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;

    public Transform dashDirectionIndicator { get; private set; }
    public Transform facingDirectionIndicator { get; private set; }
    public Vector2 currentVelocity { get; private set; }
    public int facingDirection { get; private set; }

    private Vector2 workspace;

    #endregion

    #region Unity Functions

    protected override void Awake()
    {
        base.Awake();

        _rigidBody = GetComponentInParent<Rigidbody2D>();
        _boxCollider = GetComponentInParent<BoxCollider2D>();
        dashDirectionIndicator = transform.Find("DashDirectionIndicator");
        facingDirectionIndicator = transform.Find("FacingDirectionIndicator");

        facingDirection = 1;
    }

    #endregion

    #region Update Functions

    public void LogicUpdate() //What to do in the Update() function
    {
        currentVelocity = _rigidBody.velocity;
    }

    #endregion

    #region Utility Functions

    public void CheckIfShouldFlip(int inputX) //Check if the entity should be flipped
    {
        if (inputX != 0 && inputX != facingDirection)
        {
            Flip();
        }
    }

    public void Flip() //Flip the entity in the other direction
    {
        facingDirection *= -1;
        facingDirectionIndicator.Rotate(0f, 180f * facingDirection, 0f);
    }

    public void SquashColliderDown(float height)
    {
        SetColliderOffsetY((height - _boxCollider.size.y) / 2);
        SetColliderHeight(height);
    }

    public void SquashColliderUp(float height)
    {
        SetColliderOffsetY(-(height - _boxCollider.size.y) / 2);
        SetColliderHeight(height);
    }

    #endregion

    #region Set Functions

    public void SetVelocityX(float velocity) //Change the X velocity of an entity
    {
        workspace.Set(velocity, currentVelocity.y);
        _rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocityY(float velocity) //Change the Y velocity of an entity
    {
        workspace.Set(currentVelocity.x, velocity);
        _rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction) //Change the velocity of an entity at an angle
    {
        angle.Normalize();

        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        _rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 direction) //Change the velocity of an entity 
    {
        workspace = velocity * direction;
        _rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocityZero() //Set the velocity of an entity to 0
    {
        _rigidBody.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
    }

    public void SetColliderWidth(float width)
    {
        workspace.Set(width, _boxCollider.size.y);
        _boxCollider.size = workspace;
    }

    public void SetColliderHeight(float height)
    {
        workspace.Set(_boxCollider.size.x, height);
        _boxCollider.size = workspace;
    }

    public void SetColliderSize(float width, float height)
    {
        workspace.Set(width, height);
        _boxCollider.size = workspace;
    }

    public void SetColliderOffsetX(float offset)
    {
        Vector2 center = _boxCollider.offset;

        _boxCollider.offset.Set(center.x + offset, center.y);
    }

    public void SetColliderOffsetY(float offset)
    {
        Vector2 center = _boxCollider.offset;

        center.y += offset;

        _boxCollider.offset = center;
    }

    public void SetColliderOffset(float offsetX, float offsetY)
    {
        Vector2 center = _boxCollider.offset;

        _boxCollider.offset.Set(center.x + offsetX, center.y + offsetY);
    }

    #endregion
}