using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public Color hoverColor;
	public Color notEnoughMoneyColor;
    public Vector3 positionOffset;

	[HideInInspector]
	public GameObject turret;
	[HideInInspector]
	public GameObject preview;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	private Renderer rend;
	private Color startColor;
	private bool keepHighlight = false;

	BuildManager buildManager;

	void Start ()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;

		buildManager = BuildManager.instance;
    }

	public Vector3 GetBuildPosition ()
	{
		return transform.position + positionOffset;
	}

	void OnMouseDown ()
	{
		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

		if (BuildManager.instance.HasSelectedNode)
		{
			BuildManager.instance.DeselectNode();
		}

		if (turret != null)
		{
			buildManager.UpgradeTurretOnNode(this);
			return;
		}

		if (!buildManager.CanBuild) {
			return;
		}
		BuildTurret(buildManager.GetTurretToBuild());
		BuildManager.instance.SetTurretToBuild(null);
	}

	void BuildTurret (TurretBlueprint blueprint)
	{
		if (PlayerStats.Money < blueprint.cost)
		{
			buildManager.notEnoughMoneyText.SetActive(true);
			StartCoroutine(disableNotEnoughtMoneyText());
			return;
		}
		PlayerStats.Money -= blueprint.cost;

		GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;
		Destroy(preview);

		turretBlueprint = blueprint;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);
	}

	public void UpgradeTurret (int turretId)
	{
		//new turret
		TurretBlueprint blueprint = BuildManager.instance.identityMap[turretId];

		if (PlayerStats.Money < blueprint.upgradeCost)
		{
			buildManager.notEnoughMoneyText.SetActive(true);
			StartCoroutine(disableNotEnoughtMoneyText());
			return;
		}

		PlayerStats.Money -= blueprint.upgradeCost;
		Destroy(turret);

		GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;
		turretBlueprint = blueprint;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 2f);
	}

	public void SellTurret()
	{
		//sell amount TBC
		PlayerStats.Money += turretBlueprint.cost;
		Destroy(turret);
		turretBlueprint = null;
		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 2f);
	}

	void OnMouseEnter ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (!buildManager.CanBuild)
        {
			return;
		}
        if (turret != null)
        {
			rend.material.color = notEnoughMoneyColor;
			return;
        }
		if (buildManager.HasMoney && buildManager.GetTurretToBuild() != null)
		{
			rend.material.color = hoverColor;
			BuildManager.instance.SetSelectedNode(this);
			preview = (GameObject)Instantiate(buildManager.GetTurretToBuild().preview, GetBuildPosition(), Quaternion.identity);
		}
        else
		{
			rend.material.color = notEnoughMoneyColor;
		}
	}

	void OnMouseExit ()
	{
        if (!keepHighlight)
        {
			rend.material.color = startColor;
			Destroy(preview);
		}
    }


    public void DestroyPreview()
    {
		Destroy(preview);
    }

    public void HighlightNode()
    {
		keepHighlight = true;
		rend.material.color = hoverColor;
	}

    public void removeHighlight()
    {
		keepHighlight = false;
		rend.material.color = startColor;
	}

	IEnumerator disableNotEnoughtMoneyText()
    {
		yield return new WaitForSeconds(3.0f);
		buildManager.notEnoughMoneyText.SetActive(false);
    }
}
