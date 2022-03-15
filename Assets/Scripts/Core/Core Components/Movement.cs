using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    [SerializeField]
    private Transform facingDirectionIndicator;
    [SerializeField]
    private Transform shotgun;

    public Rigidbody2D rigidBody { get; private set; }

    public int facingDirection { get; private set; }

    public Vector2 currentVelocity { get; private set; }

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        rigidBody = GetComponentInParent<Rigidbody2D>();

        facingDirection = 1;
    }

    public void LogicUpdate()
    {
        currentVelocity = rigidBody.velocity;
    }

    public void CheckIfShouldFlip(int inputX)
    {
        if (inputX != 0 && inputX != facingDirection)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        facingDirectionIndicator.Rotate(0.0f, 180.0f, 0.0f);
        shotgun.transform.position += new Vector3(facingDirection * 0.5f, 0f, 0f);
    }

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, currentVelocity.y);
        rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(currentVelocity.x, velocity);
        rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();

        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = velocity * direction;
        rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocityZero()
    {
        rigidBody.velocity = Vector2.zero;
        currentVelocity = Vector2.zero;
    }
    #endregion
}
