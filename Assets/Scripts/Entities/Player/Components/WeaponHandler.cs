using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponHandler : CoreComponent
{
    [SerializeField] private Weapon _startingWeapon;
    [SerializeField] private List<Weapon> _weapons = new List<Weapon>();

    private Weapon _currentWeapon;
    private TempShootScript weaponTemp;

    //Unity Awake
    private void Awake()
    {
        Weapon[] foundWeapons = GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in foundWeapons)
        {
            AddWeapon(weapon);
        }

        weaponTemp = GetComponentInChildren<TempShootScript>();
    }

    //Update the current weapon's logic (Update)
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _currentWeapon.LogicUpdate();
    }

    //Initialize the weapon handler
    public override void Initialize(EntityGeneric entity)
    {
        base.Initialize(entity);

        _currentWeapon = _startingWeapon;
    }

    //Add a weapon to the weapon list
    public void AddWeapon(Weapon weapon)
    {
        if (!_weapons.Contains(weapon))
        {
            _weapons.Add(weapon);
        }
    }

    //Flip the current weapon
    public void FlipWeapon(int facingDirection)
    {
        float flipAngle = (facingDirection == -1) ? 180f : 0f;

        _currentWeapon.transform.localRotation = Quaternion.Euler(flipAngle, flipAngle, 0f);
    }
}
