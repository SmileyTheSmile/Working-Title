using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour {
    private readonly List<CoreComponent> _coreComponents = new List<CoreComponent>();

    private void Awake() {
        CoreComponent[] components = GetComponentsInChildren<CoreComponent>();

        foreach (CoreComponent component in components) {
            AddComponent(component);
            component.Initialize(this);
        }

        foreach (CoreComponent component in components)
            component.SetupConnections();
    }

    private void Update() {
        foreach (CoreComponent component in _coreComponents)
            component.LogicUpdate();
    }

    private void FixedUpdate() {
        foreach (CoreComponent component in _coreComponents)
            component.PhysicsUpdate();
    }

    public void AddComponent(CoreComponent component) {
        if (!_coreComponents.Contains(component))
            _coreComponents.Add(component);
    }

    public T GetCoreComponent<T>() where T : CoreComponent {
        var component = _coreComponents.OfType<T>().FirstOrDefault();

        if (component)
            return component;

        Debug.LogWarning($"{typeof(T)} component not found.");

        return null;

    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent {
        value = GetCoreComponent<T>();

        return value;
    }
}