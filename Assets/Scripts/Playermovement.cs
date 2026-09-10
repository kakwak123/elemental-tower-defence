using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _gravity = 9.81f;
    [SerializeField]
    private float _jumpSpeed = 3.5f;
    [SerializeField]
    private float _doubleJumpMultiplier = 0.5f;

    [SerializeField]
    private float mapStartX = -47;
    [SerializeField]
    private float mapEndX = -16;
    [SerializeField]
    private float mapStartZ = 0;
    [SerializeField]
    private float mapEndZ = 30;

    private CharacterController _controller;

    private float _directionY;

    private bool _canDoubleJump = false;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = transform.right * horizontalInput + transform.forward * verticalInput;
        if (CameraSwitch.isOnPlan)
        {
           direction = Vector3.left * horizontalInput + Vector3.back * verticalInput;
           RaycastHit _hit;
           Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           if (Physics.Raycast(_ray, out _hit))
           {
              transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
           }
        }

        if (_controller.isGrounded)
        {
            _canDoubleJump = true;

            if (Input.GetButtonDown("Jump"))
            {
                _directionY = _jumpSpeed;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && _canDoubleJump)
            {
                _directionY = _jumpSpeed * _doubleJumpMultiplier;
                _canDoubleJump = false;
            }
        }

        _directionY -= _gravity * Time.deltaTime;

        direction.y = _directionY;

        _controller.Move(direction * _moveSpeed * Time.deltaTime);

        // Restrict player position
        if (transform.position.z > mapEndZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, mapEndZ);
        }
        if (transform.position.x > mapEndX)
        {
            transform.position = new Vector3(mapEndX, transform.position.y, transform.position.z);
        }
        if (transform.position.z < mapStartZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, mapStartZ);
        }
        if (transform.position.x < mapStartX)
        {
            transform.position = new Vector3(mapStartX, transform.position.y, transform.position.z);
        }
    }
}
