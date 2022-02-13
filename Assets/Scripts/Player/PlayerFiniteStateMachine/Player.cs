using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public PlayerJumpState playerJumpState { get; private set; }
    public PlayerLandState playerLandState { get; private set; }
    public PlayerInAirState playerInAirState { get; private set; }
    public PlayerWallSlideState playerWallSlideState { get; private set; }
    public PlayerWallGrabState playerWallGrabState { get; private set; }
    public PlayerWallClimbState playerWallClimbState { get; private set; }
    public PlayerWallJumpState playerWallJumpState { get; private set; }
    public PlayerLedgeClimbState playerLedgeClimbState { get; private set; }

    public PlayerInputHandler inputHandler { get; private set; }

    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }

    public Vector2 currentVelocity { get; private set; }
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

    public int facingDirection { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        wallJumpAngle = (Vector2)(Quaternion.Euler(0, 0, playerData.wallJumpAngle) * Vector2.right);

        playerIdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        playerMoveState = new PlayerMoveState(this, stateMachine, playerData, "move");
        playerJumpState = new PlayerJumpState(this, stateMachine, playerData, "inAir");
        playerInAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
        playerLandState = new PlayerLandState(this, stateMachine, playerData, "land");
        playerWallSlideState = new PlayerWallSlideState(this, stateMachine, playerData, "wallSlide");
        playerWallClimbState = new PlayerWallClimbState(this, stateMachine, playerData, "wallClimb");
        playerWallGrabState = new PlayerWallGrabState(this, stateMachine, playerData, "wallGrab");
        playerWallJumpState = new PlayerWallJumpState(this, stateMachine, playerData, "inAir");
        playerLedgeClimbState = new PlayerLedgeClimbState(this, stateMachine, playerData, "ledgeClimb");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidBody = GetComponent<Rigidbody2D>();

        facingDirection = 1;

        stateMachine.Initialize(playerIdleState);
    }

    private void Update()
    {
        currentVelocity = rigidBody.velocity;
        stateMachine.CurrentState.LogicUpdate();
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
        RaycastHit2D hitY = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
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
