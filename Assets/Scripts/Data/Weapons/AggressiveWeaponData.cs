using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/Weapon Data/Aggressive Weapon Data")]


public class AggressiveWeaponData : WeaponData
{
    [SerializeField] private WeaponAttackDetails[] _attackDetails;

    public WeaponAttackDetails[] attackDetails { get => _attackDetails; private set => _attackDetails = value; }

    private void OnEnable()
    {
        amountOfAttacks = _attackDetails.Length;

        movementSpeed = new float[amountOfAttacks];

        for (int i = 0; i < amountOfAttacks; i++)
        {
            movementSpeed[i] = _attackDetails[i].movementSpeed;
        }
    }
}
