using UnityEngine;

public abstract class PlayerState : GenericState
{
    //TODO Fix CoreComponent references missing on second compile of the game
    protected ConditionManager conditionManager
    { get => _conditionManager ?? _entity.GetCoreComponent(ref _conditionManager); }
    private ConditionManager _conditionManager;

    protected VisualController visualController
    { get => _visualController ?? _entity.GetCoreComponent(ref _visualController); }
    private VisualController _visualController;

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
