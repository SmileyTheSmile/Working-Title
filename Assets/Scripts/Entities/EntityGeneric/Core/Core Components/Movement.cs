using UnityEngine;

public class Movement : CoreComponent
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }
    private CollisionSenses _collisionSenses;

    private Combat combat
    { get => _combat ?? core.GetCoreComponent(ref _combat); }
    private Combat _combat;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;

    public PlayerCrouchingForm crouchingForm;
    public Transform facingDirectionIndicator { get; private set; }
    public Vector2 currentVelocity { get; private set; }
    public Vector2 defaultSize { get; private set; }
    
    public int facingDirection { get; private set; }
    public bool canSetVelocity;

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        rigidBody = GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponentInParent<BoxCollider2D>();

        facingDirectionIndicator = transform.Find("FacingDirectionIndicator");

        defaultSize = boxCollider.size;
        crouchingForm = PlayerCrouchingForm.normal;

        facingDirection = 1;
        canSetVelocity = true;
    }

    public override void LogicUpdate() //What to do in the Update() function
    {
        currentVelocity = rigidBody.velocity;
    }

    public void Flip() //Flip the entity in the other direction
    {
        facingDirection *= -1;

        facingDirectionIndicator.Rotate(0f, 180f * facingDirection, 0f);
        combat?.FlipCurrentWeapon(facingDirection);
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
            combat?.currentWeapon.Rotate(180f * facingDirection, 0f, 0f);
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
        || (!collisionSenses.WallFront)) && !collisionSenses.Ceiling)
        {
            ResetColliderHeight(biggerHeight, smallerHeight);
        }
    }

    public void ResetColliderHeight(float biggerHeight, float smallerHeight)
    {
        crouchingForm = PlayerCrouchingForm.normal;

        boxCollider.size = defaultSize;
        boxCollider.offset = Vector2.zero;

        collisionSenses.MoveCeilingCheck(biggerHeight, smallerHeight, defaultSize.y);
    }

    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        crouchingForm = PlayerCrouchingForm.crouchingDown;

        float height = boxCollider.size.y * smallerHeight;

        SetColliderOffsetY((height - boxCollider.size.y) / 2);
        SetColliderSize(boxCollider.size.x, height);

        collisionSenses.MoveCeilingCheck(smallerHeight, biggerHeight, defaultSize.y);
    }

    public void SquashColliderUp(float heightFraction)
    {
        Vector2 center = boxCollider.offset;
        float height = boxCollider.size.y * heightFraction;

        workspace.x = boxCollider.size.x;
        workspace.y = height;
        center.y -= ((height - boxCollider.size.y) / 2);
        //center.y -= (height - _boxCollider.size.y) / 2;

        boxCollider.size = workspace;
        boxCollider.offset = center;
        //SetOffsetY(-(height - _boxCollider.size.y) / 2);
        //SetColliderHeight(height);
    }

    //Change the drag of the entity
    public void SetDrag(float drag)
    {
        rigidBody.drag = drag;
    }

    public void SetVelocityX(float velocity) //Change the X velocity of an entity
    {
        workspace.Set(velocity, currentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity) //Change the Y velocity of an entity
    {
        workspace.Set(currentVelocity.x, velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction) //Change the velocity of an entity at an angle
    {
        angle.Normalize();

        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction) //Change the velocity of an entity 
    {
        workspace = velocity * direction;
        SetFinalVelocity();
    }

    public void SetVelocityZero() //Set the velocity of an entity to 0
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void SetFinalVelocity()
    {
        if (canSetVelocity)
        {
            rigidBody.velocity = workspace;
            currentVelocity = workspace;
        }
    }

    public void AddForceAtAngle(float force, float angle)
    {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        rigidBody.AddForce(dir * force, ForceMode2D.Impulse);
    }

    public void SetColliderSize(float width, float height)
    {
        workspace.Set(width, height);

        boxCollider.size = workspace;
    }

    public void SetColliderOffsetY(float offsetY)
    {
        workspace = boxCollider.offset;

        workspace.y += offsetY;

        boxCollider.offset = workspace;
    }

    public void SetOffset(float offsetX, float offsetY)
    {
        workspace = boxCollider.offset;

        workspace.x += offsetX;
        workspace.y += offsetY;

        boxCollider.offset = workspace;
    }
}

public enum PlayerCrouchingForm
{
    normal,
    crouchingDown,
    crouchingUp
}