using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    // Only for ranged weapons
    public GameObject projectilePrefab;
    public Transform firePoint; // Where the bullets are created

    public float fireRate = 0.5f;
    public float bulletSpeed = 30f;
    public float lifeTime = 3f;


    void Start()
    {
        if (isRanged)
        {
            if (!projectilePrefab)
                Debug.Log("No projectile assigned to ranged weapon. Please assign a projectile.");
            if (!firePoint)
                Debug.Log("No firePoint designated. Please specify a firepoint for " + name);
        }
    }


    public override void Attack()
    {
        Fire();
    }


    private void Fire()
    {
        GameObject bullet = Instantiate(projectilePrefab);

        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GameManager.playerRef.GetComponent<Collider>());
        // Physics.IgnoreCollision(bullet.GetComponent<Collider>(), firePoint.parent.GetComponent<Collider>());

        // Set bullet position and rotation
        bullet.transform.position = firePoint.position;
        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

        // bullet.transform.rotation = Quaternion.LookRotation(GameManager.playerRef.transform.rotation.eulerAngles);
        // bullet.transform.rotation = Quaternion.Euler(Vector3.forward);

        // bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
        bullet.GetComponent<Rigidbody>().AddForce(GameManager.playerRef.transform.forward * bulletSpeed, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, lifeTime));
    }


    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
