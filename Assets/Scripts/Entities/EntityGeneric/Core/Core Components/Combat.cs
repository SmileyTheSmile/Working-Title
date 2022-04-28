using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }
    private Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Stats stats
    { get => _stats ?? core.GetCoreComponent(ref _stats); }

    private CollisionSenses _collisionSenses;
    private Movement _movement;
    private Stats _stats;

    public Transform currentWeapon;
    private TempShootScript weapon;

    [SerializeField]
    private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive;
    private float knockbackStartTime;

    protected override void Awake()
    {
        weapon = GetComponentInChildren<TempShootScript>();
    }

    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount)
    {
        stats?.DecreaseHealth(amount);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        movement?.SetVelocity(strength, angle, direction);
        movement.canSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if ((isKnockbackActive && movement.currentVelocity.y <= 0.0f && collisionSenses.Ground) || (Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            movement.canSetVelocity = true;
        }
    }

    public void FlipCurrentWeapon(int facingDirection)
    {
        currentWeapon.Rotate(0f, 180f * facingDirection, 0f);
    }
}