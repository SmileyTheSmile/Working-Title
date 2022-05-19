using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour
{
    //List of all core components
    private readonly List<CoreComponent> _coreComponents = new List<CoreComponent>();

    //Unity Awake
    private void Awake()
    {
        var components = GetComponentsInChildren<CoreComponent>();

        foreach (CoreComponent component in components)
        {
            AddComponent(component);
            component.Initialize(this);
        }
    }

    //Logic update of all core components (Update)
    public void LogicUpdate()
    {
        foreach (CoreComponent component in _coreComponents)
        {
            //component.LogComponentInfo();
            component.LogicUpdate();
        }
    }

    //Physics update of all core components (FixedUpdate)
    public void PhysicsUpdate()
    {
        foreach (CoreComponent component in _coreComponents)
        {
            component.PhysicsUpdate();
        }
    }

    //Add a core component to the core
    public void AddComponent(CoreComponent component)
    {
        if (!_coreComponents.Contains(component))
        {
            _coreComponents.Add(component);
        }
    }

    //Returns the first element of generic type T in coreComponents list
    public T GetCoreComponent<T>() where T:CoreComponent
    {
        var component = _coreComponents.OfType<T>().FirstOrDefault();

        if (component == null)
        {
            Debug.LogWarning($"{typeof(T)} component not found on {transform.parent.name}");
        }

        return component;
    }

    //Returns the first element of generic type T in coreComponents list
    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();

        return value;
    }

    private void SetupStates()
    {
        /*idleState = new PlayerIdleState(this, playerData, "idle");
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
        secondaryAttackState = new PlayerAttackState(this, playerData, "attack");*/

        //GetCoreComponent<FiniteStateMachine>().Initialize();
    }
}