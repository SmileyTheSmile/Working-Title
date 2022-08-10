using UnityEngine;

public class Movement : CoreComponent
{
    public Vector2 CurrentVelocity { get => _rigidBody.velocity; }
    public Vector2 ColliderSize { get => _boxCollider.size; }
    public Vector2 DefaultSize { get; private set; }
    public bool CanSetVelocity { get; set; }

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;

    public override void Initialize(EntityCore entity)
    {
        base.Initialize(entity);

        _rigidBody = GetComponentInParent<Rigidbody2D>();
        _rigidBody.velocity = Vector2.zero;
        CanSetVelocity = true;

        _boxCollider = GetComponentInParent<BoxCollider2D>();
        DefaultSize = _boxCollider.size;
    }

    public void SetVelocityAtAngle(float velocity, Vector2 angle, int facingDirection)
    {
        angle.Normalize();
        SetFinalVelocity(new Vector2(angle.x * velocity * facingDirection, angle.y * velocity));
    }

    public void SetFinalVelocity(Vector2 velocity)
    {
        if (CanSetVelocity)
            _rigidBody.velocity = velocity;
    }

    public void AddForceAtAngle(float force, float angle)
    {
        Vector2 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        _rigidBody.AddForce(dir * force, ForceMode2D.Impulse);
    }

    public void SetDrag(float value) => _rigidBody.drag = value;
    public void SetVelocityZero() => SetFinalVelocity(Vector2.zero);
    public void SetVelocity(float velocity, Vector2 facingDirection) => SetFinalVelocity(velocity * facingDirection);
    public void SetVelocityX(float velocity) => SetFinalVelocity(new Vector2(velocity, _rigidBody.velocity.y));
    public void SetVelocityY(float velocity) => SetFinalVelocity(new Vector2(_rigidBody.velocity.x, velocity));
    public void AddForce(Vector2 direction, ForceMode2D forceMode) => _rigidBody.AddForce(direction, forceMode);
    public void AddForceX(float velocity, ForceMode2D forceMode) => _rigidBody.AddForce(new Vector2(velocity, 0), forceMode);
    public void AddForceY(float velocity, ForceMode2D forceMode) => _rigidBody.AddForce(new Vector2(0, velocity), forceMode);
    public void SetColliderSize(Vector2 size) => _boxCollider.size = size;
    public void SetColliderSize(float width, float height) => _boxCollider.size = new Vector2(width, height);
    public void SetColliderSizeX(float width) => _boxCollider.size = new Vector2(width, _boxCollider.size.y);
    public void SetColliderSizeY(float height) => _boxCollider.size = new Vector2(_boxCollider.size.x, height);
    public void SetColliderOffset(Vector2 offset) => _boxCollider.offset = offset;
    public void SetColliderOffset(float offsetX, float offsetY) => _boxCollider.offset = new Vector2(offsetX, offsetY);
    public void SetColliderOffsetX(float offsetX) => _boxCollider.offset = new Vector2(offsetX, _boxCollider.offset.y);
    public void SetColliderOffsetY(float offsetY) => _boxCollider.offset = new Vector2(_boxCollider.offset.x, offsetY);
}
