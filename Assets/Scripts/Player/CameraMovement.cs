using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5.0f;
    public float followSpeed = 10.0f;
    public float maxVerticalAngle = 80.0f;
    public float minVerticalAngle = -80.0f;
    public float maxVerticalHeight = 10.0f;
    public Vector3 offset = new Vector3(0, 3, -5);

    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleRotation();
        HandleFollow();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void HandleRotation()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        mouseY = Mathf.Clamp(mouseY, minVerticalAngle, maxVerticalAngle);

        mouseY = Mathf.Clamp(mouseY, -Mathf.Atan2(maxVerticalHeight, offset.z) * Mathf.Rad2Deg, maxVerticalAngle);

        Quaternion camRotation = Quaternion.Euler(mouseY, mouseX, 0);
        Vector3 desiredPosition = target.position + camRotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 2);
    }

    void HandleFollow()
    {
        
    }
}
