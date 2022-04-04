using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShootScript : MonoBehaviour
{
    public Transform gun;
    public Transform cursor;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public ParticleSystem impactEffect;
    public Animator animator;
    public PlayerInputHandler inputHandler;
    public LineRenderer lineRenderer;
    public ParticleSystem bulletParticles;
    public TrailRenderer bulletTrail;
    public LayerMask layerMask;
    private float shotDelay = 0.2f;
    public bool addBulletSpread;
    private float lastShotTime;
    private int pelletNum = 5;
    private Vector2 direction;
    private Vector2 bulletSpread = new Vector2(-0.1f, 0.1f);
    private Vector2 mousePositionInput;
    //public float fireRate;
    //private float damage = 20f;
    //private float readyForNextShot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //mousePositionInput = inputHandler.mousePositionInput;
        mousePositionInput = cursor.position;
        direction = mousePositionInput - (Vector2)firePoint.position;

        FaceMouse();

        if (inputHandler.attackInputs[(int)CombatInputs.primary])
        {
            /*if (Time.time > readyForNextShot)
            {
                readyForNextShot = Time.time + 1 / fireRate;
                StartCoroutine(ShootRaycast());
            }*/

            if (lastShotTime + shotDelay < Time.time)
            {
                ShootRaycast();
            }
        }
    }

    private void FaceMouse()
    {
        gun.transform.right = direction;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //gun.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ShootProjectile()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        animator.SetTrigger("Shoot");
        Destroy(bulletInstance, 3);
    }

    private IEnumerator ShootRaycastOld()
    {
        animator.SetTrigger("Shoot");

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, mousePositionInput);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);

            /*Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.Hit(damage);
            }*/

            if (impactEffect)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
            }

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, mousePositionInput * 100);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.2f);

        lineRenderer.enabled = false;
    }

    private void ShootRaycast()
    {
        animator.SetTrigger("Shoot");

        if (bulletParticles)
        {
            bulletParticles.Play();
        }

        for (int i = 0; i < pelletNum; i++)
        {
            direction = GetDirection();

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position + Vector3.right * 0.2f, direction, Mathf.Infinity, layerMask);

            if (hit)
            {
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));
            }
        }

        lastShotTime = Time.time;
    }

    private Vector2 GetDirection()
    {
        direction = mousePositionInput - (Vector2)gun.position;

        Debug.Log(direction);

        if (addBulletSpread)
        {
            direction += new Vector2(
                Random.Range(-bulletSpread.x, bulletSpread.x),
                Random.Range(-bulletSpread.y, bulletSpread.y)
            );
        }

        Debug.Log(direction);

        return direction;
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

    /*private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(gun.position, Vector3.forward, 0.8f);
    }*/
}