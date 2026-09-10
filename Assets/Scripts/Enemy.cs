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
	private float bulletSlowTimer;
	private float bulletSlowDuration;

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

	public void Slow (float pct)
	{
		speed = startSpeed * (1f - pct);
	}

    public void IceBulletSlow(float pct, float duration)
	{
		speed = startSpeed * (1f - pct);
		bulletSlowDuration = duration;
		bulletSlowTimer = 0.0f;
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
				Destroy(onFireEffect);
			}
			timeCounter = 0.0f;
		}
        if (bulletSlowTimer >= bulletSlowDuration)
        {
			speed = startSpeed;
        }

		timeCounter += Time.deltaTime;
		bulletSlowTimer += Time.deltaTime;
	}

    IEnumerator putOutFire()
    {
		yield return new WaitForSeconds(5.0f);
		Destroy(onFireEffect);
		isOnFire = false;
	}

	void Die ()
	{
		isDead = true;
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
