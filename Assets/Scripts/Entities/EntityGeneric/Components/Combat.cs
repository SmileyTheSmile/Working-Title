using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private PlayerConditionTable _conditions;

    private Movement _movement;

    [SerializeField] private float _maxKnockbackTime = 0.2f;
    [SerializeField] private float _maxHealth;

    private bool _isKnockbackActive;
    private float _knockbackStartTime;
    private float _currentHealth;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _isKnockbackActive = false;
        _currentHealth = _maxHealth;
    }

    public override void SetupConnections()
    {
        base.SetupConnections();

        _movement = _core.GetCoreComponent<Movement>();
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
        _movement?.SetVelocityAtAngle(strength, angle, direction);
        _movement.CanSetVelocity = false;

        _isKnockbackActive = true;
        _knockbackStartTime = Time.time;
    }

    //Check if knockback should be stopped
    private void CheckKnockback()
    {
        if ((_isKnockbackActive && _movement.CurrentVelocity.y <= 0.0f && _conditions.IsGrounded) || (Time.time >= _knockbackStartTime + _maxKnockbackTime))
        {
            _movement.CanSetVelocity = true;

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