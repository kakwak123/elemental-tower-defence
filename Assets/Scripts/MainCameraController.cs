using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{

    public Transform target;
    public float speed;
    public Vector3 offset;
    private float time;
    void Start()
    {
        time = Time.time;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
        if (Time.time >= time + 10f)
        {
            speed = 20;
        }
    }
}
