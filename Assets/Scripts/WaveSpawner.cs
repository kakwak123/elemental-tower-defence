using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour {

	public static int EnemiesAlive = 0;

	public Wave[] waves;
	public Transform spawnPoint;
	public float timeBetweenWaves = 5f;
	private float countdown = 10f;
	public TextMeshProUGUI waveCountdownText;
	public TextMeshProUGUI statusText;
	public GameManager gameManager;
	private int waveIndex = 0;

	// EnemiesAlive is static, so it outlives the scene. Enemies still on the map
	// when the player retries are destroyed by the scene unload without ever
	// running Die() or EndPath(), leaving the count permanently above zero -- at
	// which point Update() below returns on every frame and no wave ever starts
	// again. Clearing it here is what makes Retry actually work.
	void Start ()
	{
		EnemiesAlive = 0;
	}

	void Update ()
	{
		if (EnemiesAlive > 0)
		{
			return;
		}

		if (waveIndex == waves.Length)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}

		if (countdown <= 0f)
		{
			statusText.text = string.Format("Wave {0}", waveIndex + 1);
			StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
			return;
		}
		else
        {
			statusText.text = string.Format("Wave Countdown: {0:00.00}", countdown);
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
		//waveCountdownText.text = string.Format("Wave Countdown: {0:00.00}", countdown);
		

	}

	IEnumerator SpawnWave ()
	{
		PlayerStats.Rounds++;
		Wave wave = waves[waveIndex];
		EnemiesAlive = wave.count;

		for (int i = 0; i < wave.count; i++)
		{
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds(1f / wave.rate);
		}

		waveIndex++;
	}

	void SpawnEnemy (GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

}
