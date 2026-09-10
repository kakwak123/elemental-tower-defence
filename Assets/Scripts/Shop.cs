using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	BuildManager buildManager;
    public GameObject shopUI;
	public GameObject shopIcon;
    public float transitTime = 0.5f;
	private bool isDisplayed = true;

    public Text BulletTurretCostText;
	public Text LaserTurretCostText;
	public Text MissileTurretCostText;

	void Start()
	{
		buildManager = BuildManager.instance;
        //hide shop menu
		shopUI.transform.localScale = new Vector3(1, 1, 1);
		BulletTurretCostText.text = "$" + buildManager.bulletTurret.cost.ToString();
		LaserTurretCostText.text = "$" + buildManager.laserTurret.cost.ToString();
		MissileTurretCostText.text = "$" + buildManager.arrowTurret.cost.ToString();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
			OpenShopMenu();
		}
		if (isDisplayed)
        {
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SelectBulletTurret();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				SelectArrowTurret();
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				SelectLaserTurret();
			}
		}

	}

    public void OpenShopMenu()
    {
        if (!isDisplayed)
        {
		    LeanTween.scale(shopUI, new Vector3(1, 1, 1), transitTime);
		}
        else
        {
			LeanTween.scale(shopUI, new Vector3(0, 0, 0), transitTime);
		}
		isDisplayed = !isDisplayed;
	}

	public void SelectBulletTurret ()
	{
		buildManager.SelectTurretToBuild(buildManager.bulletTurret);
	}

	public void SelectArrowTurret()
	{
		buildManager.SelectTurretToBuild(buildManager.arrowTurret);
	}

	public void SelectLaserTurret()
	{
		buildManager.SelectTurretToBuild(buildManager.laserTurret);
	}
}
