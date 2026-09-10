using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradeUI;
    public GameObject closeButtonUI;
    public float transitTime = 0.0f;
    private bool isDisplayed = false;

    void Start()
    {
        upgradeUI.transform.localScale = new Vector3(0, 0, 0);
        closeButtonUI.transform.localScale = new Vector3(0, 0, 0);
    }

    public void OpenUpgradeMenu()
    {
        if (!isDisplayed)
        {
            LeanTween.scale(upgradeUI, new Vector3(1, 1, 1), transitTime);
            LeanTween.scale(closeButtonUI, new Vector3(1, 1, 1), transitTime);
            isDisplayed = true;
        }
    }

    public void CloseUpgradeMenu()
    {
        if (isDisplayed)
        {
            LeanTween.scale(upgradeUI, new Vector3(0, 0, 0), transitTime);
            LeanTween.scale(closeButtonUI, new Vector3(0, 0, 0), transitTime);
            isDisplayed = false;
        }
    }
}
