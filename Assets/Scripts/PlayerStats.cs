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

    void Start()
	{
		Money = startMoney;
		Lives = startLives;
		Rounds = 0;
	}
	void Update()
    {
		life.text = "" + Lives;
		gem.text = "" + Money;
	}
}
