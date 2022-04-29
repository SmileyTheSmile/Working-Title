using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour
{
    //List of all core components
    private readonly List<CoreComponent> coreComponents = new List<CoreComponent>();

    //Logic update of all core components
    public void LogicUpdate()
    {
        foreach (CoreComponent component in coreComponents)
        {
            component.LogicUpdate();
        }
    }

    //Add a core component to the core
    public void AddComponent(CoreComponent component)
    {
        if (!coreComponents.Contains(component))
        {
            coreComponents.Add(component);
        }
    }

    //Returns the first element of generic type T in coreComponents list
    public T GetCoreComponent<T>() where T:CoreComponent
    {
        var component = coreComponents.OfType<T>().FirstOrDefault();

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