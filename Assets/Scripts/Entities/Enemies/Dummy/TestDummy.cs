using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject hitParticles;
    private Animator animator;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Damage(float amount)
    {
        Debug.Log($"{amount}");

        if (hitParticles != null)
        {
            Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        }
        
        animator.SetTrigger("damage");
        //Destroy(gameObject);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        SetVelocity(strength, angle, direction);
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction) //Change the velocity of an entity at an angle
    {
        angle.Normalize();

        rigidBody.velocity = new Vector2(angle.x * velocity * direction, angle.y * velocity);
    }
}
