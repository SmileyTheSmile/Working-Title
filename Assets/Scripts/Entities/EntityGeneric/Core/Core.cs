using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> coreComponents = new List<CoreComponent>();

    public void Awake()
    {

    }

    public void LogicUpdate()
    {
        foreach (CoreComponent component in coreComponents)
        {
            component.LogicUpdate();
        }
    }

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
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
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