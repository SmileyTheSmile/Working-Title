using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShootScript : MonoBehaviour
{
    protected Movement movement
    { get => _movement ?? core.GetCoreComponent(ref _movement); }
    private Movement _movement;

    protected PlayerInputHandler inputHandler
    { get => _inputHandler ?? core.GetCoreComponent(ref _inputHandler); }
    private PlayerInputHandler _inputHandler;

    [SerializeField] private Transform gun;
    [SerializeField] private Transform primary;
    [SerializeField] private Transform cursor;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector2 bulletSpread;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem muzzleFlashParticles;
    [SerializeField] private ParticleSystem impactEffect;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private float screenShakeTime = 0.1f;
    [SerializeField] private float screenShakeIntensity = 1f;
    [SerializeField] private float shotDelay = 0.2f;
    [SerializeField] private int pelletNum = 5;

    private Vector2 bulletDirection;
    private Vector2 gunDirection;
    private Vector3 mousePositionInput;

    private Core core;
    private Animator animator;
    private bool addBulletSpread;
    private float lastShotTime;
    private float angle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        core = GetComponentInParent<Core>();

        addBulletSpread = (bulletSpread != Vector2.zero);
    }

    private void Update()
    {
        mousePositionInput = inputHandler.mousePositionInput;

        HandleAiming();
        ShootRaycast();
    }

    private void HandleAiming()
    {
        gunDirection = (mousePositionInput - primary.position).normalized;
        
        angle = Vector2.SignedAngle(Vector2.right, gunDirection);
        movement.CheckFacingDirection(mousePositionInput, core.transform.position);
        primary.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ShootProjectile()
    {
        animator.SetTrigger("Shoot");

        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Destroy(bulletInstance, 3);
    }

    private void ShootRaycast()
    {
        if (!(inputHandler._attackInputs[(int)CombatInputs.primary]))
            return;

        if (!(lastShotTime + shotDelay < Time.time))
            return;

        movement?.AddForceAtAngle(10f, angle - 180);

        animator.SetTrigger("Shoot");

        if (muzzleFlashParticles)
        {
            muzzleFlashParticles.Play();
        }

        for (int i = 0; i < pelletNum; i++)
        {
            bulletDirection = GetBulletDirection();

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position + Vector3.right * 0.2f, bulletDirection, Mathf.Infinity, layerMask);

            StartCoroutine(SpawnTrail(Instantiate(bulletTrail, firePoint.position, Quaternion.identity), hit));

            if (hit)
            {
                if (impactEffect)
                {
                    Instantiate(impactEffect, transform.position, transform.rotation);
                }
            }
        }

        CinemachineCameraShake.Instance.ShakeCamera(screenShakeIntensity, screenShakeTime);

        lastShotTime = Time.time;
    }

    private Vector2 GetBulletDirection()
    {
        bulletDirection = (mousePositionInput - firePoint.position).normalized;

        if (addBulletSpread)
        {
            bulletDirection += new Vector2(
                Random.Range(-bulletSpread.x, bulletSpread.x),
                Random.Range(-bulletSpread.y, bulletSpread.y)
            );
        }

        return bulletDirection;
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

        if (impactEffect)
        {
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.point));
        }

        Destroy(trail.gameObject, trail.time);
    }
}