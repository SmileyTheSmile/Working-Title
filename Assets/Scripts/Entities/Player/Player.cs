using UnityEngine;

public partial class Player : EntityGeneric
{
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerInAirState inAirState { get; private set; }
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

    private void Start()
    {
        SetupStates();
    }

    //Create all the player states
    private void SetupStates()
    {
        idleState = new PlayerIdleState(this, playerData, "idle");
        moveState = new PlayerMoveState(this, playerData, "move");
        jumpState = new PlayerJumpState(this, playerData, "inAir");
        inAirState = new PlayerInAirState(this, playerData, "inAir");
        landState = new PlayerLandState(this, playerData, "land");

        wallSlideState = new PlayerWallSlideState(this, playerData, "wallSlide");
        wallClimbState = new PlayerWallClimbState(this, playerData, "wallClimb");
        wallGrabState = new PlayerWallGrabState(this, playerData, "wallGrab");
        wallJumpState = new PlayerWallJumpState(this, playerData, "inAir");
        ledgeClimbState = new PlayerLedgeClimbState(this, playerData, "ledgeClimb");

        crouchIdleState = new PlayerCrouchIdleState(this, playerData, "crouchIdle");
        crouchMoveState = new PlayerCrouchMoveState(this, playerData, "crouchMove");
        crouchJumpState = new PlayerCrouchJumpState(this, playerData, "crouchInAir");
        crouchInAirState = new PlayerCrouchInAirState(this, playerData, "crouchInAir");
        crouchLandState = new PlayerCrouchLandState(this, playerData, "crouchLand");

        primaryAttackState = new PlayerAttackState(this, playerData, "attack");
        secondaryAttackState = new PlayerAttackState(this, playerData, "attack");
        
        stateMachine.Initialize(idleState);
    }

    //Draw gizmos
    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(this.transform.position, Vector3.forward, 0.4f);
    }
}
