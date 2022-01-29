using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveRightState playerMoveRightState { get; private set; }
    public PlayerMoveLeftState playerMoveLeftState { get; private set; }
    public PlayerJumpState playerJumpState { get; private set; }
    public PlayerLandState playerLandState { get; private set; }
    public PlayerInAirState playerInAirState { get; private set; }
    public PlayerWallSlideState playerWallSlideState { get; private set; }
    public PlayerWallGrabState playerWallGrabState { get; private set; }
    public PlayerWallClimbState playerWallClimbState { get; private set; }

    public PlayerInputHandler inputHandler { get; private set; }

    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public Vector2 currentVelocity { get; private set; }
    public int facingDirection { get; private set; }

    private Vector2 workspace;

    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        playerIdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        playerMoveRightState = new PlayerMoveRightState(this, stateMachine, playerData, "moveRight");
        playerMoveLeftState = new PlayerMoveLeftState(this, stateMachine, playerData, "moveLeft");
        playerJumpState = new PlayerJumpState(this, stateMachine, playerData, "inAir");
        playerInAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
        playerLandState = new PlayerLandState(this, stateMachine, playerData, "land");
        playerWallSlideState = new PlayerWallSlideState(this, stateMachine, playerData, "wallSlide");
        playerWallClimbState = new PlayerWallClimbState(this, stateMachine, playerData, "wallClimb");
        playerWallGrabState = new PlayerWallGrabState(this, stateMachine, playerData, "wallGrab");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidBody = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(playerIdleState);

        facingDirection = 1;
    }

    private void Update()
    {
        currentVelocity = rigidBody.velocity;
        facingDirection = inputHandler.normalizedInputX;
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

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    private void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishedTrigger() => stateMachine.CurrentState.AnimationFinishedTrigger();
}
