using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxToWeapon : MonoBehaviour
{
    private OldAggressiveWeapon weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<OldAggressiveWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        weapon.AddToDetected(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        weapon.RemoveFromDetected(collider);
    }
}
