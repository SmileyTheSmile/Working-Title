public abstract class PlayerState : GenericState
{
    protected PlayerInputHandler inputHandler
    { get => _inputHandler ?? core.GetCoreComponent(ref _inputHandler); }
    private PlayerInputHandler _inputHandler;

    protected Player player;
    protected PlayerData playerData;

    public PlayerState(Player player, FiniteStateMachine stateMachine, string animBoolName, PlayerData playerData)
    : base(stateMachine, animBoolName)
    {
        this.player = player;
        this.playerData = playerData;

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
