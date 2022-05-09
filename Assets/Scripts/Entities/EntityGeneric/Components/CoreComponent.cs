using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core _core;

    //Initialize the component
    public virtual void Initialize(Core core)
    {
        this._core = core;
    }
    
    
    //Update the component's logic (Update)
    public virtual void LogicUpdate() { }
    //Update the component's physics (FixedUpdate)
    public virtual void PhysicsUpdate() { }
    //Log the component's important info
    public virtual void LogComponentInfo() { }
}
