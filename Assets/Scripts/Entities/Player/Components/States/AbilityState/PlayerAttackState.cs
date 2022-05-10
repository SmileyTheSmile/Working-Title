using UnityEngine;

[CreateAssetMenu(fileName = "Player Attack State", menuName = "States/Player/Ability/Attack State")]

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon _weapon;

    private int _inputX;

    private float _velocityToSet;

    private bool _setVelocity;
    private bool _shouldCheckFlip;

    public PlayerAttackState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        _setVelocity = false;

        _weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        _weapon.ExitWeapon();
    }

    public override void DoActions()
    {
        base.DoActions();

        if (_shouldCheckFlip)
        {
            _inputX = inputHandler.normalizedInputX;
        }

        movement?.CheckMovementDirection(_inputX);

        if (_setVelocity)
        {
            movement?.SetVelocityX(_velocityToSet * movement.movementDirection);
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        _isAbilityDone = true;
    }

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
        _weapon.InitializeWeapon(this, core);
    }

    public void SetPlayerVelocity(float velocity)
    {
        movement?.SetVelocityX(velocity * movement.movementDirection);

        _velocityToSet = velocity;
        _setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        _shouldCheckFlip = value;
    }
}
