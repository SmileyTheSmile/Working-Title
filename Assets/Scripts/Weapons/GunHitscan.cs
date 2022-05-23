using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHitscan : Gun
{
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected TrailRenderer _bulletTrail;
    [SerializeField] protected ParticleSystem _impactEffect;
    [SerializeField] protected Vector2 _bulletSpread = new Vector2(0.1f, 0.1f);
    [SerializeField] protected int _pelletNum = 5;

    protected bool _addBulletSpread;

    protected override void Awake()
    {
        base.Awake();

        _addBulletSpread = (_bulletSpread != Vector2.zero);
    }

    protected override void Shoot()
    {
        for (int i = 0; i < _pelletNum; i++)
        {
            SpawnBullet();
        }
    }

    private void SpawnBullet()
    {
        Vector2 bulletDirection = GetBulletDirection();

        RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, bulletDirection, Mathf.Infinity, _layerMask);

        StartCoroutine(SpawnTrail(Instantiate(_bulletTrail, _firePoint.position, Quaternion.identity), hit));

        if (hit)
        {
            if (_impactEffect)
            {
                Instantiate(_impactEffect, transform.position, transform.rotation);
            }
        }
    }

    private Vector2 GetBulletDirection()
    {
        Vector2 bulletDirection = (_mousePosition - _firePoint.position).normalized;

        if (_addBulletSpread)
            bulletDirection = AddBulletSpread(bulletDirection);

        return bulletDirection;
    }

    private Vector2 AddBulletSpread(Vector2 bulletDirection)
    {
        return bulletDirection + new Vector2(
            Random.Range(-_bulletSpread.x, _bulletSpread.x),
            Random.Range(-_bulletSpread.y, _bulletSpread.y)
        );
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit2D hit)
    {
        float time = 0;

        Vector2 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector2.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;

        SpawnImpactEffect(hit);

        Destroy(trail.gameObject, trail.time);
    }

    private void SpawnImpactEffect(RaycastHit2D hit)
    {
        if (_impactEffect)
        {
            Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.point));
        }
    }
}
