using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static bool GameIsOver;

	public GameObject gameOverUI;
	public GameObject completeLevelUI;
	public float transitTime = 1f;
	public LeanTweenType leanTweenType;

	private void Start()
	{
		GameIsOver = false;
	}

	// Update is called once per frame
	private void Update() {
		if (GameIsOver)
			return;

		if (PlayerStats.Lives <= 0)
		{
			EndGame();
		}
	}

	private void EndGame()
	{
		GameIsOver = true;
		gameOverUI.SetActive(true);
	}

    public void WinLevel()
	{
		GameIsOver = true;
		completeLevelUI.SetActive(true);
	}

}
