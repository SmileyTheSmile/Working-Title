using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveRightState playerMoveRightState { get; private set; }
    public PlayerMoveLeftState playerMoveLeftState { get; private set; }

    public PlayerInputHandler inputHandler { get; private set; }

    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public Vector2 currentVelocity { get; private set; }
    public int facingDirection { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    private Vector2 workspace;

    private void Awake() {
        stateMachine = new PlayerStateMachine();

        playerIdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        playerMoveRightState = new PlayerMoveRightState(this, stateMachine, playerData, "moveRight");
        playerMoveLeftState = new PlayerMoveLeftState(this, stateMachine, playerData, "moveLeft");
    }

    private void Start() {
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidBody = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(playerIdleState);

        facingDirection = 1;
    }

    private void Update() {
        stateMachine.CurrentState.LogicUpdate();
        currentVelocity = rigidBody.velocity;
    }

    private void FixedUpdate() {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocityX(float velocity) {
        workspace.Set(velocity, currentVelocity.y);
        rigidBody.velocity = workspace;
        currentVelocity = workspace;
    }
}
