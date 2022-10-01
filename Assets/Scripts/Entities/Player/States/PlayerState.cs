using UnityEngine;

public abstract class PlayerState : GenericState
{
    protected TemporaryComponent _temporaryComponent;
    protected VisualController _visualController;

    [SerializeField] protected PlayerData _playerData;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _temporaryComponent = _core.GetCoreComponent<TemporaryComponent>();
        _visualController = _core.GetCoreComponent<VisualController>();
    }
    
    //What to do when entering the state
    public override void Enter()
    {
        base.Enter();

        _visualController.SetAnimationBool(_animBoolName, true);
    }

    //What to do when exiting the state
    public override void Exit()
    {
        base.Exit();

        _visualController.SetAnimationBool(_animBoolName, false);
    }
}
