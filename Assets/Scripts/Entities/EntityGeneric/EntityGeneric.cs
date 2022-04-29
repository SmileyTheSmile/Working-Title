using UnityEngine;

public class EntityGeneric : MonoBehaviour
{
    public Core core
    {
        get => GenericNotImplementedError<Core>.TryGet(_core, "core");
        private set => _core = value;
    }
    private Core _core;

    protected FiniteStateMachine stateMachine
    { get => _stateMachine ?? core.GetCoreComponent(ref _stateMachine); }
    private FiniteStateMachine _stateMachine;

    protected virtual void Awake()
    {
        _core = GetComponentInChildren<Core>();
    }

    public virtual void Update()
    {
        core.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        core.PhysicsUpdate();
    }
}
