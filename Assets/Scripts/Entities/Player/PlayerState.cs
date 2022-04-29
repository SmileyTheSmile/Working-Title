public abstract class PlayerState : GenericState
{
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

        visualController.SetAnimationBool(animBoolName, true);
    }

    //What to do when exiting the state
    public override void Exit()
    {
        base.Exit();

        visualController.SetAnimationBool(animBoolName, false);
    }
}
