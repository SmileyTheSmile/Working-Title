using UnityEngine;

public class Movement : CoreComponent
{
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

    public PlayerCrouchingForm crouchingForm;
    public Transform facingDirectionIndicator { get; private set; }
    public Vector2 currentVelocity { get; private set; }
    public Vector2 defaultSize { get; private set; }
    public int facingDirection { get; private set; }

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        _rigidBody = GetComponentInParent<Rigidbody2D>();
        _boxCollider = GetComponentInParent<BoxCollider2D>();

        facingDirectionIndicator = transform.Find("FacingDirectionIndicator");

        defaultSize = _boxCollider.size;
        crouchingForm = PlayerCrouchingForm.normal;

        facingDirection = 1;
    }

    public void LogicUpdate() //What to do in the Update() function
    {
        currentVelocity = _rigidBody.velocity;
    }

    public void Flip() //Flip the entity in the other direction
    {
        facingDirection *= -1;

        facingDirectionIndicator.Rotate(0f, 180f * facingDirection, 0f);
        core.weaponHandler.FlipCurrentWeapon(facingDirection);
    }

    public void CheckIfShouldFlip(int inputX) //Check if the entity should be flipped
    {
        if (inputX != 0 && inputX != facingDirection)
        {
            Flip();
        }
    }

    public void CheckIfShouldFlip(int inputX, float mousePosX) //Check if the entity should be flipped
    {
        if ((inputX != 0 && inputX != facingDirection) || (Mathf.Sign(mousePosX) != facingDirection && inputX == 0))
        {
            Flip();
        }
        else if (Mathf.Sign(mousePosX) != facingDirection && inputX == facingDirection)
        {
            facingDirectionIndicator.Rotate(0f, 180f * facingDirection, 0f);
            core.weaponHandler.currentWeapon.Rotate(180f * facingDirection, 0f, 0f);
        }
    }

    public void CrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (crouchingForm == PlayerCrouchingForm.normal && crouchInput)
        {
            SquashColliderDown(biggerHeight, smallerHeight);
        }
    }

    public void UnCrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (((crouchingForm == PlayerCrouchingForm.crouchingDown && !crouchInput)
        || (!core.collisionSenses.WallFront)) && !core.collisionSenses.Ceiling)
        {
            ResetColliderHeight(biggerHeight, smallerHeight);
        }
    }

    public void ResetColliderHeight(float biggerHeight, float smallerHeight)
    {
        crouchingForm = PlayerCrouchingForm.normal;

        _boxCollider.size = defaultSize;
        _boxCollider.offset = Vector2.zero;

        core.collisionSenses.MoveCeilingCheck(biggerHeight, smallerHeight, defaultSize.y);
    }

    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        crouchingForm = PlayerCrouchingForm.crouchingDown;

        float height = _boxCollider.size.y * smallerHeight;

        SetColliderOffsetY((height - _boxCollider.size.y) / 2);
        SetColliderSize(_boxCollider.size.x, height);

        core.collisionSenses.MoveCeilingCheck(smallerHeight, biggerHeight, defaultSize.y);
    }

    public void SquashColliderUp(float heightFraction)
    {
        Vector2 center = _boxCollider.offset;
        float height = _boxCollider.size.y * heightFraction;

        workspace.x = _boxCollider.size.x;
        workspace.y = height;
        center.y -= ((height - _boxCollider.size.y) / 2);
        //center.y -= (height - _boxCollider.size.y) / 2;

        _boxCollider.size = workspace;
        _boxCollider.offset = center;
        //SetOffsetY(-(height - _boxCollider.size.y) / 2);
        //SetColliderHeight(height);
    }

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

    public void AddForceAtAngle(float force, float angle)
    {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        _rigidBody.AddForce(dir * force, ForceMode2D.Impulse);
    }

    public void SetColliderSize(float width, float height)
    {
        workspace.Set(width, height);
        _boxCollider.size = workspace;
    }

    public void SetColliderOffsetY(float offsetY)
    {
        workspace = _boxCollider.offset;

        workspace.y += offsetY;

        _boxCollider.offset = workspace;
    }

    public void SetOffset(float offsetX, float offsetY)
    {
        workspace = _boxCollider.offset;

        workspace.x += offsetX;
        workspace.y += offsetY;

        _boxCollider.offset = workspace;
    }
}

public enum PlayerCrouchingForm
{
    normal,
    crouchingDown,
    crouchingUp
}