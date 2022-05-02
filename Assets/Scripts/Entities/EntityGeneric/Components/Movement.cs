using UnityEngine;

public class Movement : CoreComponent
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }
    private CollisionSenses _collisionSenses;

    private WeaponHandler weaponHandler
    { get => _weaponHandler ?? core.GetCoreComponent(ref _weaponHandler); }
    private WeaponHandler _weaponHandler;

    private VisualController visualController
    { get => _visualController ?? core.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;

    public int facingDirection { get; private set; }
    public Vector2 currentVelocity { get; private set; }
    public Vector2 defaultSize { get; private set; }

    private bool canSetVelocity;
    private Vector2 workspace;
    private PlayerCrouchingForm crouchingForm;

    //Unity Awake
    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponentInParent<BoxCollider2D>();

        facingDirection = 1;
        canSetVelocity = true;
        defaultSize = boxCollider.size;
        crouchingForm = PlayerCrouchingForm.notCrouching;
    }

    //Update the current state's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        currentVelocity = rigidBody.velocity;
    }

    //Flip the entity in the other direction
    public void Flip()
    {
        facingDirection *= -1;
        
        weaponHandler?.FlipCurrentWeapon(facingDirection);
        visualController?.FlipEntity(facingDirection);
    }
    
    //Check if the entity should be flipped
    public void CheckIfShouldFlip(int inputX)
    {
        if (inputX != 0 && inputX != facingDirection)
        {
            Flip();
        }
    }
    
    //Check if the entity should be flipped
    public void CheckIfShouldFlip(int inputX, float mousePosX)
    {
        if ((inputX != 0 && inputX != facingDirection) || (Mathf.Sign(mousePosX) != facingDirection && inputX == 0)
        || (Mathf.Sign(mousePosX) != facingDirection && inputX == facingDirection))
        {
            Flip();
        }
    }

    //Set the entity crouching state to crouchingDown and squash the collider accordingly
    public void CrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (crouchingForm == PlayerCrouchingForm.notCrouching && crouchInput)
        {
            crouchingForm = PlayerCrouchingForm.crouchingDown;

            SquashColliderDown(biggerHeight, smallerHeight);
        }
    }

    //Set the entity crouching state to notCrouching and unsquash the collider accordingly
    public void UnCrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (((crouchingForm == PlayerCrouchingForm.crouchingDown && !crouchInput)
        || (!collisionSenses.WallFront)) && !collisionSenses.Ceiling)
        {
            crouchingForm = PlayerCrouchingForm.notCrouching;

            ResetColliderHeight(biggerHeight, smallerHeight);
        }
    }

    //Reset the size and offset of the collider
    public void ResetColliderHeight(float biggerHeight, float smallerHeight)
    {
        boxCollider.size = defaultSize;
        boxCollider.offset = Vector2.zero;

        collisionSenses.MoveCeilingCheck(biggerHeight, smallerHeight, defaultSize.y);
    }

    //Squash the collider downwards
    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        float height = boxCollider.size.y * smallerHeight;

        SetColliderOffsetY((height - boxCollider.size.y) / 2);
        SetColliderSize(boxCollider.size.x, height);

        collisionSenses.MoveCeilingCheck(smallerHeight, biggerHeight, defaultSize.y);
    }

    //Set the drag of the entity
    public void SetDrag(float value)
    {
        rigidBody.drag = value;
    }

    //Set wether you can set entity's velocity or not
    public void SetCanChangeVelocity(bool value)
    {
        canSetVelocity = value;
    }

    //Set the X velocity of the entity
    public void SetVelocityX(float velocity) 
    {
        workspace.Set(velocity, currentVelocity.y);
        SetFinalVelocity();
    }

    //Set the Y velocity of the entity
    public void SetVelocityY(float velocity)
    {
        workspace.Set(currentVelocity.x, velocity);
        SetFinalVelocity();
    }

    //Set the velocity of the entity at an angle with and considering facingDirection
    public void SetVelocity(float velocity, Vector2 angle, int facingDirection)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * facingDirection, angle.y * velocity);
        SetFinalVelocity();
    }

    //Set the velocity of the entity considering facingDirection
    public void SetVelocity(float velocity, Vector2 facingDirection)
    {
        workspace = velocity * facingDirection;
        SetFinalVelocity();
    }

    //Set the velocity of the entity to 0
    public void SetVelocityZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    //Set the velocity of the entity to workspace vector
    public void SetFinalVelocity()
    {
        if (canSetVelocity)
        {
            rigidBody.velocity = workspace;
            currentVelocity = workspace;
        }
    }

    //Set the size of the entity's collider
    public void SetColliderSize(float width, float height)
    {
        workspace.Set(width, height);
        boxCollider.size = workspace;
    }

    //Set the Y offset of the entity's collider
    public void SetColliderOffsetY(float offsetY)
    {
        workspace = boxCollider.offset;
        workspace.y += offsetY;
        boxCollider.offset = workspace;
    }

    //Set the X, Y offset of the entity's collider
    public void SetOffset(float offsetX, float offsetY)
    {
        workspace = boxCollider.offset;
        workspace += new Vector2(offsetX, offsetY);
        boxCollider.offset = workspace;
    }

    public void AddForceAtAngle(float force, float angle)
    {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        rigidBody.AddForce(dir * force, ForceMode2D.Impulse);
    }
}

public enum PlayerCrouchingForm
{
    notCrouching,
    crouchingDown,
    crouchingUp
}