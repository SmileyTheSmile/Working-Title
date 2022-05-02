using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : CoreComponent
{
    [SerializeField] private Transform currentWeapon;
    
    private TempShootScript weapon;

    //Unity Awake
    private void Awake()
    {
        weapon = GetComponentInChildren<TempShootScript>();
    }

    //Flip the current weapon
    public void FlipCurrentWeapon(int facingDirection)
    {
        currentWeapon.Rotate(0f, 180f * facingDirection, 0f);
    }
}
