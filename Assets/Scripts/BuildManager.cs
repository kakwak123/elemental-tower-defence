using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour {

	public static BuildManager instance;

	public TurretBlueprint bulletTurret;
	public TurretBlueprint arrowTurret;
	public TurretBlueprint laserTurret;
	public TurretBlueprint flameTurret;
	public TurretBlueprint frozenLaserTurret;
	public TurretBlueprint iceBulletTurret;
	public TurretBlueprint advancedBulletTurret;
	public TurretBlueprint explosionArrowTurret;
	public TurretBlueprint advancedFlameTurret;

	public Dictionary<int, List<int>> upgradeMap = new Dictionary<int, List<int>>();
	public Dictionary<int, TurretBlueprint> identityMap = new Dictionary<int, TurretBlueprint>();

	void Awake ()
	{
		if (instance != null)
		{
			Debug.LogError("More than one BuildManager in scene!");
			return;
		}
		instance = this;

		//initialize hashMaps
        //max 3 upgrade target for a single turret
		upgradeMap.Add(0, new List<int>() { 3, 5, 6 });
		upgradeMap.Add(1, new List<int>() { 7 });
		upgradeMap.Add(2, new List<int>() { 4 });
		upgradeMap.Add(3, new List<int>() { 8 });

		identityMap.Add(bulletTurret.id, bulletTurret);
		identityMap.Add(arrowTurret.id, arrowTurret);
		identityMap.Add(laserTurret.id, laserTurret);
		identityMap.Add(flameTurret.id, flameTurret);
		identityMap.Add(frozenLaserTurret.id, frozenLaserTurret);
		identityMap.Add(iceBulletTurret.id, iceBulletTurret);
		identityMap.Add(advancedBulletTurret.id, advancedBulletTurret);
		identityMap.Add(explosionArrowTurret.id, explosionArrowTurret);
		identityMap.Add(advancedFlameTurret.id, advancedFlameTurret);
	}

	public GameObject buildEffect;

	private TurretBlueprint turretToBuild;
	private Node selectedNode;

	private List<NodeUI> nodeUIs = new List<NodeUI>();
	public NodeUI nodeUiPrefab;
	public GameObject upgradeUI;
	public GameObject notEnoughMoneyText;

	public bool CanBuild { get { return turretToBuild != null; } }
	public bool HasSelectedNode { get { return selectedNode != null; } }
	public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

	public void UpgradeTurretOnNode(Node node)
	{
		if (selectedNode == node)
		{
			DeselectNode();
			return;
		}
		selectedNode = node;
		turretToBuild = null;
		upgradeUI.GetComponent<UpgradeUI>().OpenUpgradeMenu();

		//node UI
		int turretId = node.turretBlueprint.id;
		if (!BuildManager.instance.upgradeMap.ContainsKey(turretId))
        {
			NodeUI nodeUI = Instantiate(nodeUiPrefab, upgradeUI.transform.position, Quaternion.identity) as NodeUI;
			nodeUI.SetTarget(node);
			nodeUI.transform.parent = upgradeUI.transform;
			nodeUIs.Add(nodeUI);
            return;
		}

		foreach (int id in upgradeMap[turretId])
        {
			NodeUI nodeUI = Instantiate(nodeUiPrefab, upgradeUI.transform.position, Quaternion.identity) as NodeUI;
			nodeUI.SetTarget(node, id);
			nodeUI.transform.parent = upgradeUI.transform;
			nodeUIs.Add(nodeUI);
		}
	}

	public void DeselectNode()
	{
		if (selectedNode != null)
		{
			selectedNode.removeHighlight();
			selectedNode = null;
		}
		upgradeUI.GetComponent<UpgradeUI>().CloseUpgradeMenu();
		foreach (Transform child in upgradeUI.transform)
		{
			Destroy(child.gameObject);
		}
	}

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
			turretToBuild = null;
            if (selectedNode != null)
            {
				selectedNode.DestroyPreview();
                DeselectNode();
            }
		}
	}

    public void SelectTurretToBuild (TurretBlueprint turret)
	{
		turretToBuild = turret;
		DeselectNode();
	}

	public TurretBlueprint GetTurretToBuild ()
	{
		return turretToBuild;
	}

	public void SetTurretToBuild(TurretBlueprint bluePrint)
	{
		turretToBuild = bluePrint;
	}

    public void SetSelectedNode(Node node)
    {
		selectedNode = node;
	}
}
