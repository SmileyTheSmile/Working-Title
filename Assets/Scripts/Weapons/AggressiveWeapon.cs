using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected AggressiveWeaponData agressiveWeaponData;

    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();

    protected override void Awake()
    {
        base.Awake();

        if (weaponData.GetType() == typeof(AggressiveWeaponData))
        {
            agressiveWeaponData = (AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for weapon");
        }
    }

    public void AddToDetected(Collider2D collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if (damageable != null)
        {
            detectedDamageables.Add(damageable);
        }

        IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();

        if (knockbackable != null)
        {
            detectedKnockbackables.Add(knockbackable);
        }
    }

    public void RemoveFromDetected(Collider2D collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if (damageable != null)
        {
            detectedDamageables.Remove(damageable);
        }

        IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();

        if (knockbackable != null)
        {
            detectedKnockbackables.Remove(knockbackable);
        }
    }

    private void CheckMeleeAttack()
    {
        WeaponAttackDetails details = agressiveWeaponData.attackDetails[attackCounter];

        foreach (IDamageable item in detectedDamageables.ToList())
        {
            item.Damage(details.damageAmount);
        }

        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(details.knockbackAngle, details.knockbackStrength, movement._movementDir);
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }
}
