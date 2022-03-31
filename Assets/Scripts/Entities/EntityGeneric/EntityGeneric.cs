using UnityEngine;

public class EntityGeneric : MonoBehaviour
{
    #region Entity Components

    public Animator animator
    {
        get
        {
            if (_animator)
            {
                return _animator;
            }

            Debug.Log("Missing Animator on " + transform.parent.name);

            return null;
        }
        private set => _animator = value;
    }

    public Core core
    {
        get
        {
            if (_core)
            {
                return _core;
            }

            Debug.Log("Missing Core on " + transform.parent.name);

            return null;
        }
        private set => _core = value;
    }

    public FiniteStateMachine stateMachine { get; private set; }

    protected Animator _animator;
    protected Core _core;

    #endregion

    #region Utility Variables

    private Vector2 workspace;

    #endregion

    #region Unity Functions

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _core = GetComponentInChildren<Core>();

        stateMachine = new FiniteStateMachine();
    }

    protected virtual void Update()
    {
        _core.LogicUpdate();

        stateMachine.currentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    protected virtual void Start() { }

    #endregion

    #region Animation Functions

    protected virtual void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    protected virtual void AnimationFinishedTrigger() => stateMachine.currentState.AnimationFinishedTrigger();

    #endregion
}
