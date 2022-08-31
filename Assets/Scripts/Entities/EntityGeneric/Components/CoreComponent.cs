using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core _core;

    //Initialize the component
    public virtual void Initialize(Core entity) => this._core = entity;
    //Connect the component to other components if needed
    public virtual void SetupConnections() {}
    //Update the component's logic (Update)
    public virtual void LogicUpdate() { }
    //Update the component's physics (FixedUpdate)
    public virtual void PhysicsUpdate() { }
    //Log the component's important info
    public virtual void LogComponentInfo() { }
}
