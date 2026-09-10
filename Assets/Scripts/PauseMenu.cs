using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject ui;
	public GameObject gameOver;
	public GameObject gameClear;
	public string menuSceneName = "MainMenu";
	public SceneFader sceneFader;

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && gameClear.activeSelf == false && gameOver.activeSelf == false)
		{
			Cursor.lockState = CursorLockMode.None;
			if (BuildManager.instance.HasSelectedNode)
            {
				BuildManager.instance.DeselectNode();
				return;
            }
			Toggle();
		}
	}

	public void Toggle ()
	{
		ui.SetActive(!ui.activeSelf);

		if (ui.activeSelf)
		{
			Time.timeScale = 0f;
		} else
		{
			Time.timeScale = 1f;
		}
	}

	public void Retry ()
	{
		Toggle();
		sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

	public void Menu ()
	{
		Toggle();
		sceneFader.FadeTo(menuSceneName);
	}

}
