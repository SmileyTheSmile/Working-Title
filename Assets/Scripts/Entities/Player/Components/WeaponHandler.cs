using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : CoreComponent
{
    protected ConditionManager conditionManager
    { get => _conditionManager ?? _entity.GetCoreComponent(ref _conditionManager); }
    private ConditionManager _conditionManager;

    [SerializeField] private Transform currentWeapon;
    
    private TempShootScript weapon;

    //Unity Awake
    private void Awake()
    {
        //Time.timeScale = playerData.holdTimeScale;
        weapon = GetComponentInChildren<TempShootScript>();
    }

    //Flip the current weapon
    public void FlipWeapon(int facingDirection)
    {
        float flipAngle = (facingDirection == -1) ? 180f : 0f;

        currentWeapon.localRotation = Quaternion.Euler(flipAngle, flipAngle, 0f);
    }
}
