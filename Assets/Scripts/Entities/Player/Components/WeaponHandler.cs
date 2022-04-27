using UnityEngine;

public class WeaponHandler : CoreComponent
{
    public Transform currentWeapon;
    private TempShootScript weapon;

    protected override void Awake()
    {
        weapon = GetComponentInChildren<TempShootScript>();
    }

    public void FlipCurrentWeapon(int facingDirection)
    {
        currentWeapon.Rotate(0f, 180f * facingDirection, 0f);
    }
}