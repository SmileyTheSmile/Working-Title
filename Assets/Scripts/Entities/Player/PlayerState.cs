public abstract class PlayerState : GenericState
{
    protected PlayerInputHandler inputHandler
    { get => _inputHandler ?? core.GetCoreComponent(ref _inputHandler); }
    private PlayerInputHandler _inputHandler;

    protected Player _player;
    protected PlayerData _playerData;

    public PlayerState(Player player, string animBoolName, PlayerData playerData)
    : base(animBoolName)
    {
        this._player = player;
        this._playerData = playerData;

        core = player.core;
    }
    
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
