using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    private CollisionSenses collisionSenses
    { get => _collisionSenses ?? _core.GetCoreComponent(ref _collisionSenses); }
    private CollisionSenses _collisionSenses;

    private Movement movement
    { get => _movement ?? _core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    [SerializeField] private float _maxKnockbackTime = 0.2f;
    [SerializeField] private float _maxHealth;

    private bool _isKnockbackActive = false;
    private float _knockbackStartTime;
    private float _currentHealth;

    //Unity Awake
    private void Awake()
    {
        _currentHealth = _maxHealth;
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

        _isKnockbackActive = true;
        _knockbackStartTime = Time.time;
    }

    //Check if knockback should be stopped
    private void CheckKnockback()
    {
        if ((_isKnockbackActive && movement._currentVelocity.y <= 0.0f && collisionSenses._groundCheck.value) || (Time.time >= _knockbackStartTime + _maxKnockbackTime))
        {
            movement?.SetCanChangeVelocity(true);

            _isKnockbackActive = false;
        }
    }

    //Decrease the health of entity
    public void DecreaseHealth(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Debug.Log("Health is 0");
        }
    }

    //Increase the health of entity
    public void IncreaseHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
    }
}