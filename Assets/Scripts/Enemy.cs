using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public float startSpeed = 10f;

	[HideInInspector]
	public float speed;

	public float startHealth = 100;
	private float health;

	public int worth = 50;

	public GameObject deathExplosion;
	public GameObject dropItem;
	public GameObject flamePrefab;

	[Header("Unity Stuff")]
	public Image healthBar;

	private bool isDead = false;
	private bool isOnFire = false;

	// Slows are modelled as a single effect with a strength and a remaining
	// lifetime. Continuous sources (the laser) refresh it every frame; one-shot
	// sources (ice bullets) set a real duration. See ApplySlow.
	private const float ContinuousSlowRefresh = 0.15f;
	private float slowFactor = 0f;
	private float slowRemaining = 0f;

	private float timeCounter = 0;
	private float burnDamage = 0;
	private GameObject onFireEffect;
	public float heightOffset = 1.7f;

	void Start ()
	{
		speed = startSpeed;
		health = startHealth;
	}

	public void TakeDamage (float amount)
	{
		health -= amount;
		healthBar.fillAmount = health / startHealth;

		if (health <= 0 && !isDead)
		{
			Die();
		}
	}

	// Called every frame by a turret that is currently beaming this enemy.
	// The short refresh window keeps the slow alive while the beam holds and
	// lets it lapse shortly after the beam breaks off.
	public void Slow (float pct)
	{
		ApplySlow(pct, ContinuousSlowRefresh);
	}

	public void IceBulletSlow(float pct, float duration)
	{
		ApplySlow(pct, duration);
	}

	// The strongest active slow wins. A weaker source cannot shorten or weaken
	// a stronger one that is still running, but may take over once it lapses.
	private void ApplySlow (float pct, float duration)
	{
		if (pct > slowFactor || slowRemaining <= 0f)
		{
			slowFactor = pct;
			slowRemaining = duration;
		}
		else if (Mathf.Approximately(pct, slowFactor))
		{
			slowRemaining = Mathf.Max(slowRemaining, duration);
		}

		speed = startSpeed * (1f - slowFactor);
	}

	public void setOnFire(float damage)
	{
		if (!isOnFire)
        {
			burnDamage = damage;
		    onFireEffect = (GameObject)Instantiate(flamePrefab, transform.position + new Vector3(0.0f, heightOffset, 0.0f), Quaternion.identity);
			StartCoroutine(putOutFire());
			isOnFire = true;
		}
	}

    private void Update()
    {
        if (isOnFire && timeCounter >= 1.0f)
        {
			health -= burnDamage;
			healthBar.fillAmount = health / startHealth;
			onFireEffect.transform.position = transform.position + new Vector3(0.0f, heightOffset, 0.0f);

			if (health <= 0 && !isDead)
			{
				Die();
				return;
			}
			timeCounter = 0.0f;
		}

		// Only expire a slow that is actually running. The previous version reset
		// speed whenever timer >= duration, which was true from frame one for
		// every enemy and silently cancelled the laser's slow each frame.
		if (slowRemaining > 0f)
		{
			slowRemaining -= Time.deltaTime;

			if (slowRemaining <= 0f)
			{
				slowFactor = 0f;
				speed = startSpeed;
			}
		}

		timeCounter += Time.deltaTime;
	}

    IEnumerator putOutFire()
    {
		yield return new WaitForSeconds(5.0f);

		if (onFireEffect != null)
		{
			Destroy(onFireEffect);
			onFireEffect = null;
		}

		isOnFire = false;
	}

	void Die ()
	{
		isDead = true;

		// Every death path has to release the flame effect, not just the burn
		// tick. An enemy set alight and then shot used to orphan this object in
		// the scene forever.
		if (onFireEffect != null)
		{
			Destroy(onFireEffect);
			onFireEffect = null;
		}

		Instantiate(dropItem, transform.position, Quaternion.identity);
		PlayerStats.Money += worth;

		//GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		//Destroy(effect, 5f);
		GameObject effect = (GameObject)Instantiate(deathExplosion, transform.position, Quaternion.identity);
		Destroy(effect, 1.5f);
		WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }

}
