using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Base Data")]

public class OldWeaponData : ScriptableObject
{
    public float[] movementSpeed { get; protected set; }
    public int amountOfAttacks { get; protected set; }
}

[System.Serializable]
public struct WeaponAttackDetails
{
    public string attackName;
    public float movementSpeed;
    public float damageAmount;
    public float knockbackStrength;
    public Vector2 knockbackAngle;
}