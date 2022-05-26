using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Make a better projectile gun
public class GunProjectile : Gun 
{
    [SerializeField] protected GameObject _bulletPrefab;

    protected override void CreateBullets()
    {
        GameObject bulletInstance = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        Destroy(bulletInstance, 3); 
    }
}
