using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerInAirState inAirState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallGrabState wallGrabState { get; private set; }
    public PlayerWallClimbState wallClimbState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerCrouchIdleState crouchIdleState { get; private set; }
    public PlayerCrouchMoveState crouchMoveState { get; private set; }

    public PlayerInputHandler inputHandler { get; private set; }

    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }

    public Vector2 currentVelocity { get; private set; }

    public Transform dashDirectionIndicator { get; private set; }

    private Vector2 workspace;
    public Vector2 wallJumpAngle;

    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform ledgeCheck;

    [SerializeField]
    private Transform wallCheck;

    public Vector2 mouseDirection;
    public Vector2 mouseDirectionInput;

    public int facingDirection { get; private set; }
    private float angle;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        wallJumpAngle = (Vector2)(Quaternion.Euler(0, 0, playerData.wallJumpAngle) * Vector2.right);

        idleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerData, "move");
        jumpState = new PlayerJumpState(this, stateMachine, playerData, "inAir");
        inAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
        landState = new PlayerLandState(this, stateMachine, playerData, "land");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerData, "wallSlide");
        wallClimbState = new PlayerWallClimbState(this, stateMachine, playerData, "wallClimb");
        wallGrabState = new PlayerWallGrabState(this, stateMachine, playerData, "wallGrab");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, playerData, "inAir");
        ledgeClimbState = new PlayerLedgeClimbState(this, stateMachine, playerData, "ledgeClimb");
        dashState = new PlayerDashState(this, stateMachine, playerData, "inAir");
        crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, playerData, "crouchIdle");
        crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, playerData, "crouchIdle");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidBody = GetComponent<Rigidbody2D>();
        dashDirectionIndicator = transform.Find("DashDirectionIndicator");

        facingDirection = 1;

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        currentVelocity = rigidBody.velocity;
        stateMachine.CurrentState.LogicUpdate();

        UpdateGunTemporary();
    }

    private void UpdateGunTemporary()
    {
        mouseDirectionInput = inputHandler.mouseDirectionInput;

        if (mouseDirectionInput != Vector2.zero)
        {
            mouseDirection = mouseDirectionInput;
            mouseDirection.Normalize();
        }

        angle = Vector2.SignedAngle(Vector2.right, mouseDirection);
        dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

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

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D hitX = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float distX = hitX.distance;
        workspace.Set(distX * facingDirection, 0f);
        RaycastHit2D hitY = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y + 0.15f, playerData.whatIsGround);
        float distY = hitY.distance;
        workspace.Set(wallCheck.position.x + (distX * facingDirection), ledgeCheck.position.y - distY);
        return workspace;
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
    }

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    private void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishedTrigger() => stateMachine.CurrentState.AnimationFinishedTrigger();
}
