using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret : Turret
{
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;


    // Update is called once per frame
    void Update()
    {
		if (target == null)
		{
			return;
		}

		LockOnTarget();

		if (fireCountdown <= 0f)
		{
			Shoot();
			fireCountdown = 1f / fireRate;
		}
		fireCountdown -= Time.deltaTime;


	}

	void Shoot()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();



		if (bullet != null)
			bullet.Seek(target);


	}
}
