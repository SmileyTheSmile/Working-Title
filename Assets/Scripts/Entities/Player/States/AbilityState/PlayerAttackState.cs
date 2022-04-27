using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int inputX;

    private float velocityToSet;

    private bool setVelocity;
    private bool shouldCheckFlip;

    public PlayerAttackState(Player player, FiniteStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;

        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (shouldCheckFlip)
        {
            inputX = player.inputHandler.normalizedInputX;
        }

        core.movement.CheckIfShouldFlip(inputX);

        if (setVelocity)
        {
            core.movement.SetVelocityX(velocityToSet * core.movement.facingDirection);
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        isAbilityDone = true;
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, core);
    }

    public void SetPlayerVelocity(float velocity)
    {
        core.movement.SetVelocityX(velocity * core.movement.facingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }
}
