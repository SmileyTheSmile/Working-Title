using UnityEngine;

public class Player : EntityGeneric
{
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerInAirState inAirState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerLandState landState { get; private set; }

    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallGrabState wallGrabState { get; private set; }
    public PlayerWallClimbState wallClimbState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }

    public PlayerCrouchIdleState crouchIdleState { get; private set; }
    public PlayerCrouchMoveState crouchMoveState { get; private set; }
    public PlayerCrouchJumpState crouchJumpState { get; private set; }
    public PlayerCrouchInAirState crouchInAirState { get; private set; }
    public PlayerCrouchLandState crouchLandState { get; private set; }

    public PlayerAttackState primaryAttackState { get; private set; }
    public PlayerAttackState secondaryAttackState { get; private set; }

    [SerializeField] private PlayerData playerData;

    public PlayerInputHandler inputHandler { get; private set; }
    public PlayerInventory inventory { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SetupStates();
    }

    protected override void Start()
    {
        base.Start();

        inputHandler = GetComponent<PlayerInputHandler>();
        inventory = GetComponent<PlayerInventory>();
        //Time.timeScale = playerData.holdTimeScale;

        //primaryAttackState.SetWeapon(inventory.weapons[(int)CombatInputs.primary]);
        secondaryAttackState.SetWeapon(inventory.weapons[(int)CombatInputs.primary]);

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        LogImportantInfo();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void SetupStates() //Create all the player states
    {
        idleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerData, "move");
        jumpState = new PlayerJumpState(this, stateMachine, playerData, "inAir");
        inAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
        dashState = new PlayerDashState(this, stateMachine, playerData, "inAir");
        landState = new PlayerLandState(this, stateMachine, playerData, "land");

        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerData, "wallSlide");
        wallClimbState = new PlayerWallClimbState(this, stateMachine, playerData, "wallClimb");
        wallGrabState = new PlayerWallGrabState(this, stateMachine, playerData, "wallGrab");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, playerData, "inAir");
        ledgeClimbState = new PlayerLedgeClimbState(this, stateMachine, playerData, "ledgeClimb");

        crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, playerData, "crouchIdle");
        crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, playerData, "crouchMove");
        crouchJumpState = new PlayerCrouchJumpState(this, stateMachine, playerData, "crouchInAir");
        crouchInAirState = new PlayerCrouchInAirState(this, stateMachine, playerData, "crouchInAir");
        crouchLandState = new PlayerCrouchLandState(this, stateMachine, playerData, "crouchLand");

        primaryAttackState = new PlayerAttackState(this, stateMachine, playerData, "attack");
        secondaryAttackState = new PlayerAttackState(this, stateMachine, playerData, "attack");
    }

    private void LogImportantInfo() //Log the current info about player
    {
        //core.collisionSenses.LogCurrentCollisions();
        //inputHandler.LogAllInputs();
        //stateMachine.LogCurrentState();
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(this.transform.position, Vector3.forward, 0.4f);
        UnityEditor.Handles.DrawWireDisc(this.transform.position, Vector3.forward, 0.8f);
    }
}
