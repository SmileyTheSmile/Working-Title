using System;
using UnityEngine;

public class Movement : CoreComponent
{
    private WeaponHandler weaponHandler
    { get => _weaponHandler ?? _entity.GetCoreComponent(ref _weaponHandler); }
    private WeaponHandler _weaponHandler;

    private VisualController visualController
    { get => _visualController ?? _entity.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    [SerializeField] private Transform _ceilingCheckTransform;
    [SerializeField] private CollisionCheckTransitionCondition _ceilingCheck;
    [SerializeField] private CollisionCheckTransitionCondition _wallFront;
    [SerializeField] private ScriptableInt _movementDirSO;
    [SerializeField] private PlayerData _playerData;

    public Vector2 currentVelocity { get => _rigidBody.velocity; }

    private int _facingDir;
    private bool _canSetVelocity;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private Vector2 _defaultSize;
    private PlayerCrouchingForm _crouchingForm;

    private void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody2D>();
        _boxCollider = GetComponentInParent<BoxCollider2D>();

        _rigidBody.velocity = Vector2.zero;

        _movementDirSO.value = 1;
        _facingDir = 1;
        _canSetVelocity = true;
        _defaultSize = _boxCollider.size;
        _crouchingForm = PlayerCrouchingForm.notCrouching;
    }

    public void Flip()
    {
        _facingDir *= -1;

        weaponHandler?.FlipWeapon(_facingDir);
        visualController?.FlipEntity(_facingDir);
    }
    
    //Change the movement direction of the entity based on the x input
    public void CheckMovementDirection(int inputX)
    {
        if (inputX != 0 && inputX != _movementDirSO.value)
        {
            _movementDirSO.value *= -1;
        }
    }

    //Change the facing direction of the entity based on the mouse position
    public void CheckFacingDirection(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 mouseDirection = (mousePos - playerPos).normalized;

        float angle = Vector2.SignedAngle(Vector2.right, mouseDirection);
        angle = (angle > 90) ? angle - 270 : angle + 90;

        if (Math.Sign(angle) != _facingDir)
        {
            Flip();
        }
    }

    public void CrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (_crouchingForm == PlayerCrouchingForm.notCrouching && crouchInput)
        {
            _crouchingForm = PlayerCrouchingForm.crouchingDown;

            SquashColliderDown(biggerHeight, smallerHeight);
        }
    }

    public void UnCrouchDown(float biggerHeight, float smallerHeight, bool crouchInput)
    {
        if (((_crouchingForm == PlayerCrouchingForm.crouchingDown && !crouchInput)
        || !_wallFront.value) && !_ceilingCheck.value)
        {
            _crouchingForm = PlayerCrouchingForm.notCrouching;

            ResetColliderHeight(biggerHeight, smallerHeight);
        }
    }

    public void ResetColliderHeight(float biggerHeight, float smallerHeight)
    {
        _boxCollider.size = _defaultSize;
        _boxCollider.offset = Vector2.zero;

        MoveCeilingCheck(biggerHeight, smallerHeight, _defaultSize.y);
    }

    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        float height = _boxCollider.size.y * smallerHeight;

        SetColliderOffsetY((height - _boxCollider.size.y) / 2);
        SetColliderSize(_boxCollider.size.x, height);

        MoveCeilingCheck(smallerHeight, biggerHeight, _defaultSize.y);
    }
    public void MoveCeilingCheck(float oldHeight, float newHeight, float defaultColliderHeight)
    {
        _ceilingCheckTransform.position += Vector3.up * ((oldHeight - newHeight) * defaultColliderHeight);
    }

    public void SetDrag(float value)
    {
        _rigidBody.drag = value;
    }

    public void SetCanChangeVelocity(bool value)
    {
        _canSetVelocity = value;
    }

    public void SetVelocityX(float velocity)
    {
        SetFinalVelocity(new Vector2(velocity, _rigidBody.velocity.y));
    }

    public void SetVelocityY(float velocity)
    {
        SetFinalVelocity(new Vector2(_rigidBody.velocity.x, velocity));
    }

    public void SetVelocity(float velocity, Vector2 angle, int facingDirection)
    {
        angle.Normalize();
        SetFinalVelocity(new Vector2(angle.x * velocity * facingDirection, angle.y * velocity));
    }

    public void SetVelocity(float velocity, Vector2 facingDirection)
    {
        SetFinalVelocity(velocity * facingDirection);
    }

    public void SetVelocityZero()
    {
        SetFinalVelocity(Vector2.zero);
    }
    public void SetFinalVelocity(Vector2 workspace)
    {
        if (_canSetVelocity)
        {
            _rigidBody.velocity = workspace;
        }
    }

    public void AddForceAtAngle(float force, float angle)
    {
        Vector2 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        _rigidBody.AddForce(dir * force, ForceMode2D.Impulse);
    }

    public void AddForce(Vector2 direction, ForceMode2D forceMode)
    {
        _rigidBody.AddForce(direction, forceMode);
    }

    public void AddForceX(float velocity, ForceMode2D forceMode)
    {
        _rigidBody.AddForce(new Vector2(velocity, 0), forceMode);
    }

    public void AddForceY(float velocity, ForceMode2D forceMode)
    {
        _rigidBody.AddForce(new Vector2(0, velocity), forceMode);
    }

    public void SetColliderSize(float width, float height)
    {
        Vector2 workspace = new Vector2(width, height);
        _boxCollider.size = workspace;
    }

    public void SetColliderOffsetY(float offsetY)
    {
        Vector2 workspace = _boxCollider.offset;
        workspace.y += offsetY;
        _boxCollider.offset = workspace;
    }

    public void SetOffset(float offsetX, float offsetY)
    {
        Vector2 workspace = _boxCollider.offset;
        workspace += new Vector2(offsetX, offsetY);
        _boxCollider.offset = workspace;
    }
    
    private enum PlayerCrouchingForm
    {
        notCrouching,
        crouchingDown,
        crouchingUp
    }
}
