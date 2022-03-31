using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    #region Input Variables

    private int inputX;

    #endregion

    #region Utility Variables

    private Weapon weapon;

    private float velocityToSet;

    private bool setVelocity;
    private bool shouldCheckFlip;

    #endregion

    #region State Functions

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

    #endregion

    #region Utility Functions

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this);
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

    #endregion
}
