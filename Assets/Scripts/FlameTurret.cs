using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTurret : Turret
{
    public ParticleSystem flameThrower;
    public int damageOverTime = 30;
    public int burnDamage = 30;

    void Start()
    {
        flameThrower.Stop();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update()
    {
		if (target == null)
		{
            flameThrower.Stop();
            return;
		}
		LockOnTarget();
        
        Vector3 dir = target.position - firePoint.position;
        flameThrower.transform.position = firePoint.position + dir.normalized;
        flameThrower.transform.rotation = Quaternion.LookRotation(dir);
        flameThrower.Play();
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        if (burnDamage > 0f)
        {
           targetEnemy.setOnFire(burnDamage);
        }
    }
}
