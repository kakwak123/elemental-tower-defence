using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerStats : MonoBehaviour {

	public TextMeshProUGUI life;
	public TextMeshProUGUI gem;

	public static int Money;
	public int startMoney = 400;
	public static int Lives;
	public int startLives = 20;

	public static int Rounds;

	// Last values pushed to the labels. Building the strings unconditionally every
	// frame allocated two of them per frame for values that change a few times a
	// wave, which is steady garbage for the collector to sweep up.
	private int displayedLives = int.MinValue;
	private int displayedMoney = int.MinValue;

    void Start()
	{
		Money = startMoney;
		Lives = startLives;
		Rounds = 0;
	}

	void Update()
    {
		if (Lives != displayedLives)
		{
			displayedLives = Lives;
			life.text = displayedLives.ToString();
		}

		if (Money != displayedMoney)
		{
			displayedMoney = Money;
			gem.text = displayedMoney.ToString();
		}
	}
}
