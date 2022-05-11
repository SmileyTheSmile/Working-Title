using UnityEngine;

public class EntityGeneric : MonoBehaviour
{
    public Core core
    {
        get => GenericNotImplementedError<Core>.TryGet(_core, "core");
        private set => _core = value;
    }
    private Core _core;

    public FiniteStateMachine stateMachine
    { get => _stateMachine ?? core.GetCoreComponent(ref _stateMachine); }
    private FiniteStateMachine _stateMachine;

    //Unity Awake
    protected virtual void Awake()
    {
        core = GetComponentInChildren<Core>();
    }

    //Unity Update
    public virtual void Update()
    {
        core.LogicUpdate();
    }

    //Unity FixedUpdate
    public virtual void FixedUpdate()
    {
        core.PhysicsUpdate();
    }
}
