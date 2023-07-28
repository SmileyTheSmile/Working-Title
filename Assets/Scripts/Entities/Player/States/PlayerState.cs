using UnityEngine;

public abstract class PlayerState : GenericState
{
    [SerializeField] protected PlayerStats _stats;
    
    protected Player _player;
    protected VisualController _visualController;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _player = _core.GetCoreComponent<Player>();
        _visualController = _core.GetCoreComponent<VisualController>();
    }
    
    public override void Enter()
    {
        base.Enter();

        _visualController.SetAnimationBool(_animBoolName, true);
    }

    public override void Exit()
    {
        base.Exit();

        _visualController.SetAnimationBool(_animBoolName, false);
    }
}
