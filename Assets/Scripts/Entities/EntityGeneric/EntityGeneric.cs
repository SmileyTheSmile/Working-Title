using UnityEngine;
using Events;

public class EntityGeneric : MonoBehaviour
{
    [SerializeField] private EventListener _updateEventListener;
    [SerializeField] private EventListener _fixedUpdateEventListener;

    public Core core
    {
        get => GenericNotImplementedError<Core>.TryGet(_core, "core");
        private set => _core = value;
    }
    private Core _core;

    protected FiniteStateMachine stateMachine
    { get => _stateMachine ?? core.GetCoreComponent(ref _stateMachine); }
    private FiniteStateMachine _stateMachine;

    //Unity Awake
    protected virtual void Awake()
    {
        core = GetComponentInChildren<Core>();
    }

    private void OnEnable()
    {
        _updateEventListener.OnEventHappened += LogicUpdate;
        _fixedUpdateEventListener.OnEventHappened += PhysicsUpdate;
    }

    private void OnDisable()
    {
        _updateEventListener.OnEventHappened -= LogicUpdate;
        _fixedUpdateEventListener.OnEventHappened -= PhysicsUpdate;
    }

    //Unity Update
    public virtual void LogicUpdate()
    {
        core.LogicUpdate();
    }

    //Unity FixedUpdate
    public virtual void PhysicsUpdate()
    {
        core.PhysicsUpdate();
    }
}
