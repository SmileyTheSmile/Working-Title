using UnityEngine;

[CreateAssetMenu(fileName = "Player Old Attack State", menuName = "States/Player/Ability/Old Attack State")]

public class OldPlayerAttackState : PlayerAbilityState
{
    private OldWeapon _weapon;

    private int _inputX => _temporaryComponent.NormalizedInputX;
    private int _movementDir => _temporaryComponent.MovementDir;

    private float _velocityToSet;

    private bool _setVelocity;
    private bool _shouldCheckFlip;
    
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

        _temporaryComponent.CheckMovementDirection(_inputX);

        if (_setVelocity)
        {
            _movement.SetVelocityX(_velocityToSet * _movementDir);
        }
    }

    /*public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        _isAbilityDone = true;
    }*/

    public void SetWeapon(OldWeapon weapon)
    {
        _weapon = weapon;
        _weapon.InitializeWeapon(this, _core);
    }

    public void SetPlayerVelocity(float velocity)
    {
        _movement?.SetVelocityX(velocity * _movementDir);

        _velocityToSet = velocity;
        _setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        _shouldCheckFlip = value;
    }
}
