using UnityEngine;

public class WeaponHandler : CoreComponent
{
    public Transform currentWeapon;
    public TempShootScript weapon;

    protected override void Awake()
    {
        weapon = GetComponentInChildren<TempShootScript>();
    }
}