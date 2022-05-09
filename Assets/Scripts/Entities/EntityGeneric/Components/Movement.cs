using System;
using UnityEngine;

public class Movement : CoreComponent
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? _core.GetCoreComponent(ref _collisionSenses); }
    private CollisionSenses _collisionSenses;

    private WeaponHandler weaponHandler
    { get => _weaponHandler ?? _core.GetCoreComponent(ref _weaponHandler); }
    private WeaponHandler _weaponHandler;

    private VisualController visualController
    { get => _visualController ?? _core.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;

    public int movementDirection { get; private set; }
    public int facingDirection { get; private set; }
    public Vector2 currentVelocity { get; private set; }
    public Vector2 defaultSize { get; private set; }

    private bool _canSetVelocity;
    private Vector2 _workspace;
    private PlayerCrouchingForm _crouchingForm;

    //Unity Awake
    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponentInParent<BoxCollider2D>();

        movementDirection = 1;
        facingDirection = 1;
        _canSetVelocity = true;
        defaultSize = boxCollider.size;
        _crouchingForm = PlayerCrouchingForm.notCrouching;
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

        weaponHandler?.FlipWeapon(facingDirection);
        visualController?.FlipEntity(facingDirection);
    }
    
    //Change the movement direction of the entity based on the x input
    public void CheckMovementDirection(int inputX)
    {
        if (inputX != 0 && inputX != movementDirection)
        {
            movementDirection *= -1;
        }
    }

    //Change the facing direction of the entity based on the mouse position
    public void CheckFacingDirection(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 mouseDirection = (mousePos - playerPos).normalized;

        float angle = Vector2.SignedAngle(Vector2.right, mouseDirection);
        angle = (angle > 90) ? angle - 270 : angle + 90;

        if (Math.Sign(angle) != facingDirection) //(angle > 90 || angle < -90) -left
        {
            Flip();
        }
    }

    //Set the entity crouching state to crouchingDown and squash the collider accordingly
    public void CrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (_crouchingForm == PlayerCrouchingForm.notCrouching && crouchInput)
        {
            _crouchingForm = PlayerCrouchingForm.crouchingDown;

            SquashColliderDown(biggerHeight, smallerHeight);
        }
    }

    //Set the entity crouching state to notCrouching and unsquash the collider accordingly
    public void UnCrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (((_crouchingForm == PlayerCrouchingForm.crouchingDown && !crouchInput)
        || (!collisionSenses.WallFront)) && !collisionSenses.Ceiling)
        {
            _crouchingForm = PlayerCrouchingForm.notCrouching;

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
        _canSetVelocity = value;
    }

    //Set the X velocity of the entity
    public void SetVelocityX(float velocity) 
    {
        _workspace.Set(velocity, currentVelocity.y);
        SetFinalVelocity();
    }

    //Set the Y velocity of the entity
    public void SetVelocityY(float velocity)
    {
        _workspace.Set(currentVelocity.x, velocity);
        SetFinalVelocity();
    }

    //Set the velocity of the entity at an angle with and considering facingDirection
    public void SetVelocity(float velocity, Vector2 angle, int facingDirection)
    {
        angle.Normalize();
        _workspace.Set(angle.x * velocity * facingDirection, angle.y * velocity);
        SetFinalVelocity();
    }

    //Set the velocity of the entity considering facingDirection
    public void SetVelocity(float velocity, Vector2 facingDirection)
    {
        _workspace = velocity * facingDirection;
        SetFinalVelocity();
    }

    //Set the velocity of the entity to 0
    public void SetVelocityZero()
    {
        _workspace = Vector2.zero;
        SetFinalVelocity();
    }

    //Set the velocity of the entity to workspace vector
    public void SetFinalVelocity()
    {
        if (_canSetVelocity)
        {
            rigidBody.velocity = _workspace;
            currentVelocity = _workspace;
        }
    }

    //Set the size of the entity's collider
    public void SetColliderSize(float width, float height)
    {
        _workspace.Set(width, height);
        boxCollider.size = _workspace;
    }

    //Set the Y offset of the entity's collider
    public void SetColliderOffsetY(float offsetY)
    {
        _workspace = boxCollider.offset;
        _workspace.y += offsetY;
        boxCollider.offset = _workspace;
    }

    //Set the X, Y offset of the entity's collider
    public void SetOffset(float offsetX, float offsetY)
    {
        _workspace = boxCollider.offset;
        _workspace += new Vector2(offsetX, offsetY);
        boxCollider.offset = _workspace;
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