using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurretBlueprint {

	public int id;
	public string name;
	public Sprite image;
	public GameObject preview;
	public GameObject prefab;
	public int cost;
	public string description;

	public int upgradeCost;
}
