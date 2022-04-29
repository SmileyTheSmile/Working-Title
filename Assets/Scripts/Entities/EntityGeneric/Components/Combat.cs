using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }
    private CollisionSenses _collisionSenses;

    private Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    private Stats stats
    { get => _stats ?? core.GetCoreComponent(ref _stats); }
    private Stats _stats;

    [SerializeField] private float maxKnockbackTime = 0.2f;
    [SerializeField] private Transform currentWeapon;

    private bool isKnockbackActive;
    private float knockbackStartTime;
    private TempShootScript weapon;

    //Unity Awake
    protected override void Awake()
    {
        base.Awake();

        SetupElements();
    }

    //Setup component elements
    private void SetupElements()
    {
        weapon = GetComponentInChildren<TempShootScript>();
    }

    //What to do in the Update() function
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckKnockback();
    }

    //Deal damage to entity
    public void Damage(float amount)
    {
        stats?.DecreaseHealth(amount);
    }

    //Apply knockback to entity
    public void Knockback(Vector2 angle, float strength, int direction)
    {
        movement?.SetVelocity(strength, angle, direction);
        movement?.SetCanChangeVelocity(false);

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    //Check if knockback state should be stopped
    private void CheckKnockback()
    {
        if ((isKnockbackActive && movement.currentVelocity.y <= 0.0f && collisionSenses.Ground) || (Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            movement?.SetCanChangeVelocity(true);

            isKnockbackActive = false;
        }
    }

    //Flip the current weapon
    public void FlipCurrentWeapon(int facingDirection)
    {
        currentWeapon.Rotate(0f, 180f * facingDirection, 0f);
    }
}