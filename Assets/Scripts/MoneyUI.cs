using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUI : MonoBehaviour {

	public Text moneyText;

	// Only rebuild the label when the value behind it actually changed; see the
	// note in PlayerStats.
	private int displayed = int.MinValue;

	// Update is called once per frame
	void Update () {
		if (PlayerStats.Money == displayed)
		{
			return;
		}

		displayed = PlayerStats.Money;
		moneyText.text = "$" + displayed.ToString();
	}
}
