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
    public PlayerAttackState primaryAttackState { get; private set; }
    public PlayerAttackState secondaryAttackState { get; private set; }

    public PlayerInputHandler inputHandler { get; private set; }

    public Rigidbody2D rigidBody { get; private set; }
    public BoxCollider2D movementCollider { get; private set; }
    public Animator animator { get; private set; }
    public PlayerInventory inventory { get; private set; }

    public Transform dashDirectionIndicator { get; private set; }

    public Core core { get; private set; }

    private Vector2 workspace;
    public Vector2 wallJumpAngle;

    [SerializeField]
    private PlayerData playerData;

    public Vector2 mouseDirection;
    public Vector2 mouseDirectionInput;

    [SerializeField]
    private Transform shotgun;

    private float angle;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();

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
        crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, playerData, "crouchMove");
        primaryAttackState = new PlayerAttackState(this, stateMachine, playerData, "attack");
        secondaryAttackState = new PlayerAttackState(this, stateMachine, playerData, "attack");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rigidBody = GetComponent<Rigidbody2D>();
        movementCollider = GetComponent<BoxCollider2D>();

        dashDirectionIndicator = transform.Find("DashDirectionIndicator");

        inventory = GetComponent<PlayerInventory>();
        primaryAttackState.SetWeapon(inventory.weapons[(int)CombatInputs.primary]);
        //secondaryAttackState.SetWeapon(inventory.weapons[(int)CombatInputs.secondary]);

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        core.LogicUpdate();
        stateMachine.CurrentState.LogicUpdate();
        UpdateGunTemporary();

        //LogImportantInfo();
    }

    private void LogImportantInfo()
    {
        core.collisionSenses.LogCurrentCollisions();
        Debug.Log(stateMachine.CurrentState.ToString());
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
        shotgun.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetColliderHeight(float height)
    {
        Vector2 center = movementCollider.offset;
        workspace.Set(movementCollider.size.x, height);

        center.y += ((height - movementCollider.size.y) / 2);

        movementCollider.size = workspace;
        movementCollider.offset = center;
    }



    private void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishedTrigger() => stateMachine.CurrentState.AnimationFinishedTrigger();
}
