using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public Animator animator { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }

    [SerializeField]
    public PlayerData playerData;

    private void Awake() {
        stateMachine = new PlayerStateMachine();

        playerIdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        playerMoveState = new PlayerMoveState(this, stateMachine, playerData, "move");
    }

    private void Start() {
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();

        stateMachine.Initialize(playerIdleState);
    }

    private void Update() {
        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        stateMachine.CurrentState.PhysicsUpdate();
    }
}
