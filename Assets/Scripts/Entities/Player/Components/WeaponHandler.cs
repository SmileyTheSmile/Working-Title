using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : CoreComponent
{
    private Player player;

    [SerializeField] private Transform currentWeapon;
    [SerializeField] private Weapon[] weapons;
    
    private TempShootScript weapon;

    private PlayerInventory inventory;

    //Unity Awake
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        weapon = GetComponentInChildren<TempShootScript>();
    }

    private void Start()
    {
        SetupWeapons();
        //Time.timeScale = playerData.holdTimeScale;
    }

    //Flip the current weapon
    public void FlipCurrentWeapon(int facingDirection)
    {
        float flipAngle = (facingDirection == -1) ? 180f : 0f;
        currentWeapon.rotation = Quaternion.Euler(currentWeapon.rotation.x, flipAngle, currentWeapon.rotation.z);
    }

    private void SetupWeapons()
    {
        //primaryAttackState.SetWeapon(weapons[(int)CombatInputs.primary]);
        player.secondaryAttackState.SetWeapon(weapons[(int)CombatInputs.primary]);
    }
}
