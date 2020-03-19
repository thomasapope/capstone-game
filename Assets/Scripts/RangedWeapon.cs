using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public enum GunType {Semi, Burst, Auto}

    public GameObject projectilePrefab;
    public Transform firePoint; // Where the bullets are created

    // Ranged stats
    public GunType gunType= GunType.Auto;

    public float rpm = 10;
    public float bulletSpeed = 30f;

    public float lifeTime = 3f;

    // System:
    private float secondsBetweenShots;
    private float nextPossibleShootTime;


    protected override void Start()
    {
        base.Start();

        // Check for required references
        if (!projectilePrefab)
            Debug.Log("No projectile assigned to ranged weapon. Please assign a projectile.");
        if (!firePoint)
            Debug.Log("No firePoint designated. Please specify a firepoint for " + name);

        // Set up rpm
        secondsBetweenShots = 60 / rpm;
    }


    public override void Attack()
    {
        Fire();
    }


    public override void AttackContinuous()
    {
        if (gunType == GunType.Auto)
        {
            Fire();
        }
    }


    private void Fire()
    {
        if (CanFire())
        {
            GameObject bullet = Instantiate(projectilePrefab);

            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GameManager.playerRef.GetComponent<Collider>());
            
            // Give the bullet damage
            bullet.GetComponent<BulletBehavior>().damage = damage;

            // Set bullet position and rotation
            bullet.transform.position = firePoint.position;
            Vector3 rotation = bullet.transform.rotation.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            // Give the bullet a force
            bullet.GetComponent<Rigidbody>().AddForce(GameManager.playerRef.transform.forward * bulletSpeed, ForceMode.Impulse);

            nextPossibleShootTime = Time.time + secondsBetweenShots;

            audioSource.PlayVaryPitch();

            StartCoroutine(DestroyBulletAfterTime(bullet, lifeTime));
        }
    }


    private bool CanFire()
    {
        bool canShoot = true;

        if (Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }

        return canShoot;
    }


    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
