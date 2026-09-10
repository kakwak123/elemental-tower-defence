using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayer : MonoBehaviour
{
    BuildManager buildManager;
    public shooting shoot;
    public GameObject shopUI;
    private bool isDisplayed = true;
    public float transitTime = 0.5f;
    public FMODUnity.StudioEventEmitter sound1;
    public FMODUnity.StudioEventEmitter sound2;

    // Start is called before the first frame update
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenShopMenu();
        }
        if (isDisplayed)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UpgradeDamage();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                UpgradeRate();
            }
        }
    }
    public void UpgradeDamage()
    {
        if (PlayerStats.Money < 2)
        {
            buildManager.notEnoughMoneyText.SetActive(true);
            StartCoroutine(disableNotEnoughtMoneyText());
            return;
        }
        else
        {
            PlayerStats.Money -= 2;
            shoot.damage += 3;
        }
        sound1.Play();
    }
    public void UpgradeRate()
    {
        if (PlayerStats.Money < 2)
        {
            buildManager.notEnoughMoneyText.SetActive(true);
            StartCoroutine(disableNotEnoughtMoneyText());
            return;
        }
        else
        {
            PlayerStats.Money -= 2;
            if (shoot.fireRate >= 1)
            {
                shoot.fireRate += 1f;
            }
           
        }
        sound2.Play();
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
    IEnumerator disableNotEnoughtMoneyText()
    {
        yield return new WaitForSeconds(3.0f);
        buildManager.notEnoughMoneyText.SetActive(false);
    }
}
