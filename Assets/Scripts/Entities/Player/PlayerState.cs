using UnityEngine;

public abstract class PlayerState : GenericState
{
    protected ConditionManager conditionManager
    { get => _conditionManager ?? _core.GetCoreComponent(ref _conditionManager); }
    private ConditionManager _conditionManager;

    protected VisualController visualController
    { get => _visualController ?? _core.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

    protected PlayerInputHandler inputHandler
    { get => _inputHandler ?? _core.GetCoreComponent(ref _inputHandler); }
    private PlayerInputHandler _inputHandler;

    [SerializeField] protected PlayerData _playerData;
    
    //What to do when entering the state
    public override void Enter()
    {
        base.Enter();

        visualController.SetAnimationBool(_animBoolName, true);
    }

    //What to do when exiting the state
    public override void Exit()
    {
        base.Exit();

        visualController.SetAnimationBool(_animBoolName, false);
    }
}
