using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGeneric : MonoBehaviour
{
    public float bulletSpeed = 1200f;
    public float damage = 20f;
    public Rigidbody2D rigidBody;
    public GameObject impactEffect;

    private void Start()
    {
        rigidBody.AddForce(transform.right * bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        /*Enemy enemy = hitInfo.GetComponent<Enemy1>();

        if (enemy)
        {
            enemy.Hit(damage);
        }*/

        Debug.Log(hitInfo.name);
        if (hitInfo.name != "Player")
        {
            Destroy(gameObject);
        }

        if (impactEffect)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
    }
}
