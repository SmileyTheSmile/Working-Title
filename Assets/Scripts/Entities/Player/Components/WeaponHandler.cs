using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : CoreComponent
{
    protected ConditionManager conditionManager
    { get => _conditionManager ?? _core.GetCoreComponent(ref _conditionManager); }
    private ConditionManager _conditionManager;

    private Player player;

    [SerializeField] private Transform currentWeapon;
    [SerializeField] private Weapon[] primaryWeapons;
    [SerializeField] private Weapon[] secondaryWeapons;
    
    private TempShootScript weapon;

    //Unity Awake
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        weapon = GetComponentInChildren<TempShootScript>();
    }

    private void Start()
    {
        //Time.timeScale = playerData.holdTimeScale;
        SetupWeapons();
    }

    //Flip the current weapon
    public void FlipWeapon(int facingDirection)
    {
        float flipAngle = (facingDirection == -1) ? 180f : 0f;

        currentWeapon.localRotation = Quaternion.Euler(flipAngle, flipAngle, 0f);
    }

    private void SetupWeapons()
    {
        //_core.primaryAttackState.SetWeapon(primaryWeapons[0]);
        conditionManager.secondaryAttackState.SetWeapon(secondaryWeapons[0]);
    }
}
