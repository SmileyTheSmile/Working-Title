using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core core;

    protected virtual void Awake()
    {
        core = GetComponentInParent<Core>();

        if (core == null)
        {
            Debug.LogError("There is no Core on the parent.");
        }

        core.AddComponent(this);
    }

    //Log the component's important info
    public virtual void LogComponentInfo() { }
    //Update the component's logic (Update)
    public virtual void LogicUpdate() { }
    //Update the component's physics (FixedUpdate)
    public virtual void PhysicsUpdate() { }
}
