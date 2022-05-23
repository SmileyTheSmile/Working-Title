using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Weapon
{
    [SerializeField] protected ParticleSystem _muzzleFlashParticles;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected float _screenShakeTime = 0.2f;
    [SerializeField] protected float _screenShakeIntensity = 3f;
    [SerializeField] protected float _shotDelay = 0.2f;

    protected float _lastShotTime;

    public override void Attack()
    {
        if (!(_lastShotTime + _shotDelay < Time.time))
            return;

        AddRecoilToPlayer();
        ShakeCamera();

        PlayWeaponAnimation();
        PlayMuzzleFlash();
        Shoot();

        _lastShotTime = Time.time;
    }

    protected virtual void PlayMuzzleFlash()
    {
        if (_muzzleFlashParticles)
        {
            _muzzleFlashParticles.Play();
        }
    }

    protected virtual void AddRecoilToPlayer()
    {
        movement.AddForceAtAngle(10f, _aimAngle - 180);
    }

    protected virtual void PlayWeaponAnimation()
    {
        _weaponAnimator.SetTrigger("Shoot");
    }

    protected virtual void ShakeCamera()
    {
        CinemachineCameraShake.Instance.ShakeCamera(_screenShakeIntensity, _screenShakeTime);
    }

    protected abstract void Shoot();
}
