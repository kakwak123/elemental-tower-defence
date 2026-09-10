using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret
{
	[Header("Use Laser")]
	public int damageOverTime = 30;
	public float slowAmount = 0.5f;

	public LineRenderer lineRenderer;
	public ParticleSystem impactEffect;
	public Light impactLight;


	void Update()
    {
		if (target == null)
		{
			if (lineRenderer.enabled)
			{
				lineRenderer.enabled = false;
				impactEffect.Stop();
				impactLight.enabled = false;
			}
			return;
		}

		LockOnTarget();
		Laser();
	}

	void Laser()
	{
		targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
		if (slowAmount > 0f)
        {
			targetEnemy.Slow(slowAmount);
		}

		if (!lineRenderer.enabled)
		{
			lineRenderer.enabled = true;
			impactEffect.Play();
			impactLight.enabled = true;
		}

		lineRenderer.SetPosition(0, firePoint.position);
		lineRenderer.SetPosition(1, target.position);

		Vector3 dir = firePoint.position - target.position;

		impactEffect.transform.position = target.position + dir.normalized;
		impactEffect.transform.rotation = Quaternion.LookRotation(dir);
	}
}
