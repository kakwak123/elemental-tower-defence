using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVision : MonoBehaviour
{
    public Transform playerBody;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        // get the mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // it will rotate the camera on y position (wrong variable name) and set a limit to y rotation like our head
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        // move the camera rotation up and down by the mouse input
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // rotate the whole body by the mouse input we received.
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
