using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraSwitch : MonoBehaviour
{
    public static bool isOnPlan = true;
    public GameObject planCamera;
    public GameObject playerCamera;

    private void Start()
    {
        if (isOnPlan == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
        if (Input.GetMouseButtonDown(1) && !isOnPlan) 
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0) && !isOnPlan)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        // Stop player moving on Plan view
    }

    public void SwitchCamera()
    {
        if (isOnPlan)
        {
            // switch the camera by activating/deactivating
            planCamera.SetActive(false);
            playerCamera.SetActive(true);
            // lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            isOnPlan = false;
        }
        else
        {
            planCamera.SetActive(true);
            playerCamera.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            isOnPlan = true;
        }
    }
}
