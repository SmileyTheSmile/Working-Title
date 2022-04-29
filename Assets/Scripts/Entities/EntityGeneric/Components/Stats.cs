using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] private float maxHealth;
    
    private float currentHealth;

    //Unity Awake
    protected override void Awake()
    {
        base.Awake();

        SetupStats();
    }

    //Setup all default states
    private void SetupStats()
    {
        currentHealth = maxHealth;
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
