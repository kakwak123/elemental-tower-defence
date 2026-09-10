using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] steps;
    private int index = 0;
    public Shop shop;
    public UpgradePlayer up;
    public void SkipTutorial()
    {
        shop.OpenShopMenu();
        up.OpenShopMenu();
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Time.timeScale = 0f;
        if (index == steps.Length)
        {
            shop.OpenShopMenu();
            up.OpenShopMenu();
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        for (int i = 0; i < steps.Length; i++)
        {
            if (i == index)
            {
                steps[i].SetActive(true);
            }
            else
            {
                steps[i].SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            index++;
        }
    }
}
