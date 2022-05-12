using System;
using UnityEngine;

public class Movement : CoreComponent
{
    private WeaponHandler weaponHandler
    { get => _weaponHandler ?? _core.GetCoreComponent(ref _weaponHandler); }
    private WeaponHandler _weaponHandler;

    private VisualController visualController
    { get => _visualController ?? _core.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    [SerializeField] private Transform _ceilingCheckTransform;
    [SerializeField] private CollisionCheckTransitionCondition _ceilingCheck;
    [SerializeField] private CollisionCheckTransitionCondition _wallFront;
    [SerializeField] private ScriptableInt _movementDirSO;

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;

    public int _movementDir { get; private set; }
    public int _facingDir { get; private set; }
    public Vector2 _currentVelocity { get; private set; }
    public Vector2 _defaultSize { get; private set; }

    private bool _canSetVelocity;
    private Vector2 _workspace;
    private PlayerCrouchingForm _crouchingForm;

    //Unity Awake
    private void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody2D>();
        _boxCollider = GetComponentInParent<BoxCollider2D>();

        _movementDir = 1;
        _movementDirSO.value = 1;
        _facingDir = 1;
        _canSetVelocity = true;
        _defaultSize = _boxCollider.size;
        _crouchingForm = PlayerCrouchingForm.notCrouching;
    }

    //Update the current state's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _currentVelocity = _rigidBody.velocity;
    }

    //Flip the entity in the other direction
    public void Flip()
    {
        _facingDir *= -1;

        weaponHandler?.FlipWeapon(_facingDir);
        visualController?.FlipEntity(_facingDir);
    }
    
    //Change the movement direction of the entity based on the x input
    public void CheckMovementDirection(int inputX)
    {
        if (inputX != 0 && inputX != _movementDir)
        {
            _movementDir *= -1;
            _movementDirSO.value *= -1;
        }
    }

    //Change the facing direction of the entity based on the mouse position
    public void CheckFacingDirection(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 mouseDirection = (mousePos - playerPos).normalized;

        float angle = Vector2.SignedAngle(Vector2.right, mouseDirection);
        angle = (angle > 90) ? angle - 270 : angle + 90;

        if (Math.Sign(angle) != _facingDir) //(angle > 90 || angle < -90) -left
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
        || !_wallFront.value) && !_ceilingCheck.value)
        {
            _crouchingForm = PlayerCrouchingForm.notCrouching;

            ResetColliderHeight(biggerHeight, smallerHeight);
        }
    }

    //Reset the size and offset of the collider
    public void ResetColliderHeight(float biggerHeight, float smallerHeight)
    {
        _boxCollider.size = _defaultSize;
        _boxCollider.offset = Vector2.zero;

        MoveCeilingCheck(biggerHeight, smallerHeight, _defaultSize.y);
    }

    //Squash the collider downwards
    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        float height = _boxCollider.size.y * smallerHeight;

        SetColliderOffsetY((height - _boxCollider.size.y) / 2);
        SetColliderSize(_boxCollider.size.x, height);

        MoveCeilingCheck(smallerHeight, biggerHeight, _defaultSize.y);
    }

    //Move the ceiling check point position
    public void MoveCeilingCheck(float oldHeight, float newHeight, float defaultColliderHeight)
    {
        _ceilingCheckTransform.position += Vector3.up * ((oldHeight - newHeight) * defaultColliderHeight);
    }

    //Set the drag of the entity
    public void SetDrag(float value)
    {
        _rigidBody.drag = value;
    }

    //Set wether you can set entity's velocity or not
    public void SetCanChangeVelocity(bool value)
    {
        _canSetVelocity = value;
    }

    //Set the X velocity of the entity
    public void SetVelocityX(float velocity) 
    {
        _workspace.Set(velocity, _currentVelocity.y);
        SetFinalVelocity();
    }

    //Set the Y velocity of the entity
    public void SetVelocityY(float velocity)
    {
        _workspace.Set(_currentVelocity.x, velocity);
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
            _rigidBody.velocity = _workspace;
            _currentVelocity = _workspace;
        }
    }

    //Set the size of the entity's collider
    public void SetColliderSize(float width, float height)
    {
        _workspace.Set(width, height);
        _boxCollider.size = _workspace;
    }

    //Set the Y offset of the entity's collider
    public void SetColliderOffsetY(float offsetY)
    {
        _workspace = _boxCollider.offset;
        _workspace.y += offsetY;
        _boxCollider.offset = _workspace;
    }

    //Set the X, Y offset of the entity's collider
    public void SetOffset(float offsetX, float offsetY)
    {
        _workspace = _boxCollider.offset;
        _workspace += new Vector2(offsetX, offsetY);
        _boxCollider.offset = _workspace;
    }

    public void AddForceAtAngle(float force, float angle)
    {
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        _rigidBody.AddForce(dir * force, ForceMode2D.Impulse);
    }
}

public enum PlayerCrouchingForm
{
    notCrouching,
    crouchingDown,
    crouchingUp
}