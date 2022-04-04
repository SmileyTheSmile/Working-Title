public class PlayerState : GenericState
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

    public override void Enter() //What to do when entering the state
    {
        base.Enter();

        player.animator.SetBool(animBoolName, true);
    }

    public override void Exit() //What to do when exiting the state
    {
        base.Exit();

        player.animator.SetBool(animBoolName, false);
    }
}
