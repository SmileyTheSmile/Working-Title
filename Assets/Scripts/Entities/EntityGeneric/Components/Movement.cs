using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : CoreComponent
{
    public Vector2 CurrentVelocity { get => _rigidBody.velocity; }
    public bool CanSetVelocity { get; set; }

    private Rigidbody2D _rigidBody;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.velocity = Vector2.zero;
        CanSetVelocity = true;
    }

    public void SetVelocityAtAngle(float velocity, Vector2 angle, int facingDirection)
    {
        angle.Normalize();
        SetFinalVelocity(new Vector2(angle.x * velocity * facingDirection, angle.y * velocity));
    }

    public void AddForceAtAngle(float force, float angle)
    {
        Vector2 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

        _rigidBody.AddForce(dir * force, ForceMode2D.Impulse);
    }

    public void SetDrag(float value) => _rigidBody.drag = value;
    public void SetGravity(float value) => _rigidBody.gravityScale = value;
    public void SetVelocityZero() => SetFinalVelocity(Vector2.zero);
    public void SetVelocity(float velocity, Vector2 facingDirection) => SetFinalVelocity(velocity * facingDirection);
    public void SetVelocityX(float velocity) => SetFinalVelocity(new Vector2(velocity, _rigidBody.velocity.y));
    public void SetVelocityY(float velocity) => SetFinalVelocity(new Vector2(_rigidBody.velocity.x, velocity));
    public void SetFinalVelocity(Vector2 velocity) => _rigidBody.velocity = CanSetVelocity? velocity : _rigidBody.velocity;
    public void AddForce(Vector2 direction, ForceMode2D forceMode) => _rigidBody.AddForce(direction, forceMode);
    public void AddForceX(float velocity, ForceMode2D forceMode) => _rigidBody.AddForce(new Vector2(velocity, 0), forceMode);
    public void AddForceY(float velocity, ForceMode2D forceMode) => _rigidBody.AddForce(new Vector2(0, velocity), forceMode);
}
