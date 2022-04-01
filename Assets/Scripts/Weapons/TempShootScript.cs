using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShootScript : MonoBehaviour
{
    public Transform gun;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject impactEffect;
    public Animator animator;
    public PlayerInputHandler inputHandler;
    public LineRenderer lineRenderer;
    public float fireRate;
    private float readyForNextShot;
    private float damage = 20f;
    public bool attackInput;
    private Vector2 direction;
    private Vector2 mouseDirectionInput;

    private void Update()
    {
        mouseDirectionInput = inputHandler.mouseDirectionInput;
        direction = mouseDirectionInput - (Vector2)gun.position;
        FaceMouse();

        if (inputHandler.attackInputs[(int)CombatInputs.primary])
        {
            if (Time.time > readyForNextShot)
            {
                readyForNextShot = Time.time + 1 / fireRate;
                StartCoroutine(ShootRaycast());
            }
        }
    }

    private void FaceMouse()
    {
        gun.transform.right = direction;
    }

    private void ShootProjectile()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        animator.SetTrigger("Shoot");
        Destroy(bulletInstance, 3);
    }

    private IEnumerator ShootRaycast()
    {
        animator.SetTrigger("Shoot");

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, mouseDirectionInput);

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
            lineRenderer.SetPosition(1, mouseDirectionInput * 100);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.2f);

        lineRenderer.enabled = false;
    }
}
