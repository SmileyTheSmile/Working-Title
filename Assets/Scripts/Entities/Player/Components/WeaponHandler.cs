using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponHandler : CoreComponent
{
    [SerializeField] private List<Weapon> _weapons = new List<Weapon>();

    private Weapon _currentWeapon;

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _currentWeapon.LogicUpdate();
    }

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        Weapon[] foundWeapons = GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in foundWeapons)
            AddWeapon(weapon);

        _currentWeapon = _weapons[0];
    }

    //Add a weapon to the weapon list
    public void AddWeapon(Weapon weapon)
    {
        if (!_weapons.Contains(weapon))
            _weapons.Add(weapon);
    }

    //Flip the current weapon
    public void FlipWeapon(int facingDir)
    {
        float flipAngle = (facingDir == -1) ? 180f : 0f;

        _currentWeapon.Flip(flipAngle);
    }
}
