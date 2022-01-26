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

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        playerIdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        playerMoveRightState = new PlayerMoveRightState(this, stateMachine, playerData, "moveRight");
        playerMoveLeftState = new PlayerMoveLeftState(this, stateMachine, playerData, "moveLeft");
        playerJumpState = new PlayerJumpState(this, stateMachine, playerData, "jump");
        playerInAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
        playerLandState = new PlayerLandState(this, stateMachine, playerData, "land");
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
}
