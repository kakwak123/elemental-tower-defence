using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour {

	public Text livesText;

	// Only rebuild the label when the value behind it actually changed; see the
	// note in PlayerStats.
	private int displayed = int.MinValue;

	// Update is called once per frame
	void Update () {
		if (PlayerStats.Lives == displayed)
		{
			return;
		}

		displayed = PlayerStats.Lives;
		livesText.text = displayed.ToString() + " LIVES";
	}
}
