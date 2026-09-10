using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

	public GameObject ui;
	public Text turretName;
	public Text upgradeCost;
	public Button upgradeButton;
	public Image upgradeImage;
	public Text sellCost;
	public Text descriptionText;

	private Node target;
	private TurretBlueprint bluePrint;

	public void SetTarget (Node _target, int upgradeTurretId)
    {
		target = _target;
		
		bluePrint = BuildManager.instance.identityMap[upgradeTurretId];
		turretName.text = bluePrint.name;
		upgradeCost.text = "$" + bluePrint.upgradeCost;
		descriptionText.text = bluePrint.description;
		//sell amount TBC
		sellCost.text = "$" + target.turretBlueprint.cost;
		upgradeImage.sprite = bluePrint.image;
		upgradeButton.interactable = true;
		target.HighlightNode();
		ui.SetActive(true);
	}

	public void SetTarget(Node _target)
	{
		target = _target;
		upgradeCost.text = "DONE";
		//sell amount TBC
		sellCost.text = "$" + target.turretBlueprint.cost;
		upgradeImage.enabled = false;
		upgradeButton.interactable = false;
		target.HighlightNode();
		ui.SetActive(true);
	}

	public void Hide ()
	{
		ui.SetActive(false);
		target.removeHighlight();
	}

	public void Upgrade ()
	{
		GetComponent<FMODUnity.StudioEventEmitter>().Play();
		target.UpgradeTurret(bluePrint.id);
		BuildManager.instance.DeselectNode();
	}
	public void Sell()
    {
		GetComponent<FMODUnity.StudioEventEmitter>().Play();
		target.SellTurret();
		BuildManager.instance.DeselectNode();
	}
}
