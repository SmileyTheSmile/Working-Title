using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityGeneric : MonoBehaviour
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

    //Logic update of all core components
    public void Update()
    {
        foreach (CoreComponent component in _coreComponents)
        {
            //component.LogComponentInfo();
            component.LogicUpdate();
        }
    }

    //Physics update of all core components
    public void FixedUpdate()
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
}