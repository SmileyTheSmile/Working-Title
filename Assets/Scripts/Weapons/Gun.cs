using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Weapon
{
    [SerializeField] protected ParticleSystem _muzzleFlashParticles;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected AudioSourcePlayer _audioSourcePlayer;
    [SerializeField] protected float _screenShakeTime = 0.2f;
    [SerializeField] protected float _screenShakeIntensity = 3f;
    [SerializeField] protected float _shotDelay = 0.2f;
    [SerializeField] protected float _knockbackForce = 5f;
    [SerializeField] protected float _reloadTime = 2f;
    [SerializeField] protected int _maxClipSize = 4;

    protected float _reloadStartTime;
    protected float _lastShotTime;
    protected int _currentClipSize;

    protected override void Awake()
    {
        base.Awake();

        _currentClipSize = _maxClipSize;
        _conditions.IsReloading = false;
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _conditions.CanAttack = (_lastShotTime + _shotDelay < Time.time);

        if (_conditions.IsReloading)
            CheckReload();
        else
            if (_conditions.IsPressingPrimaryAttack && _conditions.CanAttack)
                Shoot();
    }

    protected virtual void Shoot()
    {
        _lastShotTime = Time.time;
        _currentClipSize -= 1;

        CreateBullets();
        AddKnockbackToPlayer();
        ShakeCamera();
        PlayWeaponAnimation("Shoot");
        PlayMuzzleFlash();
        PlaySound();

        if (_currentClipSize == 0)
            StartReload();
    }

    protected virtual void StartReload()
    {
        _conditions.IsReloading = true;
        _reloadStartTime = Time.time;

        PlayWeaponAnimation("Reload");
    }

    protected virtual void CheckReload()
    {
        if (Time.time >= _reloadStartTime + _reloadTime)
        {
            _conditions.IsReloading = false;
            _currentClipSize = _maxClipSize;
        }
    }

    protected virtual void PlayMuzzleFlash()
    {
        if (_muzzleFlashParticles)
            _muzzleFlashParticles.Play();
    }

    protected virtual void PlaySound()
    {
        if (_audioSourcePlayer)
            _audioSourcePlayer.Play();
    }

    protected virtual void AddKnockbackToPlayer()
    {
        movement.AddForceAtAngle(_knockbackForce, _aimAngle - 180);
    }

    protected virtual void PlayWeaponAnimation(string animName)
    {
        _weaponAnimator.SetTrigger(animName);
    }

    protected virtual void ShakeCamera()
    {
        CinemachineCameraShake.Instance.ShakeCamera(_screenShakeIntensity, _screenShakeTime);
    }

    protected abstract void CreateBullets();
}
