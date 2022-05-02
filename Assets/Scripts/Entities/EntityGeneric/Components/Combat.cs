using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? core.GetCoreComponent(ref _collisionSenses); }
    private CollisionSenses _collisionSenses;

    private Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    [SerializeField] private float maxKnockbackTime = 0.2f;
    [SerializeField] private float maxHealth;

    private bool isKnockbackActive = false;
    private float knockbackStartTime;
    private float currentHealth;

    //Unity Awake
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    //Update the component's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckKnockback();
    }

    //Deal damage to entity
    public void Damage(float amount)
    {
        DecreaseHealth(amount);
    }

    //Apply knockback to entity
    public void Knockback(Vector2 angle, float strength, int direction)
    {
        movement?.SetVelocity(strength, angle, direction);
        movement?.SetCanChangeVelocity(false);

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    //Check if knockback should be stopped
    private void CheckKnockback()
    {
        if ((isKnockbackActive && movement.currentVelocity.y <= 0.0f && collisionSenses.Ground) || (Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            movement?.SetCanChangeVelocity(true);

            isKnockbackActive = false;
        }
    }

    //Decrease the health of entity
    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Health is 0");
        }
    }

    //Increase the health of entity
    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}