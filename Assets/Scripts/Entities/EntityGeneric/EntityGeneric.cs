using UnityEngine;

public class EntityGeneric : MonoBehaviour
{
    public Core core
    {
        get => GenericNotImplementedError<Core>.TryGet(_core, "core");
        private set => _core = value;
    }
    private Core _core;

    public Animator animator
    {
        get => GenericNotImplementedError<Animator>.TryGet(_animator, "animator");
        private set => _animator = value;
    }
    private Animator _animator;

    protected FiniteStateMachine stateMachine;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _core = GetComponentInChildren<Core>();

        stateMachine = new FiniteStateMachine();
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        core.LogicUpdate();
        stateMachine.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    protected virtual void AnimationTrigger() => stateMachine.AnimationTrigger();
    protected virtual void AnimationFinishedTrigger() => stateMachine.AnimationFinishedTrigger();
}
