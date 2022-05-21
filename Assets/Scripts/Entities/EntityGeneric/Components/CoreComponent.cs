using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected EntityGeneric _entity;

    //Initialize the component
    public virtual void Initialize(EntityGeneric entity)
    {
        this._entity = entity;
    }
    
    //Update the component's logic (Update)
    public virtual void LogicUpdate() { }
    //Update the component's physics (FixedUpdate)
    public virtual void PhysicsUpdate() { }
    //Log the component's important info
    public virtual void LogComponentInfo() { }
}
