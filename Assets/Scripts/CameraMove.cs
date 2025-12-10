using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public bool travarMouse = true;

    [Header("Mouse Settings")]
    public float sensibilidade = 2.0f;
    public float minY = -40f;
    public float maxY = 80f;

    [Header("Camera Offset Settings")]
    public float distance = 10f;
    public float minDistance = 5f;
    public float maxDistance = 20f;
    public float zoomSpeed = 2f;
    public float height = 5f;
    public float smoothSpeed = 0.125f;

    private float currentYaw = 0f;
    private float currentPitch = 20f;

    void Start()
    {
        if (travarMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidade;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidade;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minY, maxY);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, height, -distance);

        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.LookAt(player);
    }
}