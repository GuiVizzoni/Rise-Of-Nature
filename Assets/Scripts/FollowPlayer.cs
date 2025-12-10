using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;
    public Vector3[] cameraOffsets = {
        new Vector3(0, 5, -10),   // Trás
        new Vector3(10, 5, 0),    // Direita
        new Vector3(0, 5, 10),    // Frente
        new Vector3(-10, 5, 0)    // Esquerda
    };

    public int currentOffsetIndex { get; private set; } = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentOffsetIndex = (currentOffsetIndex + 1) % cameraOffsets.Length;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentOffsetIndex = (currentOffsetIndex - 1 + cameraOffsets.Length) % cameraOffsets.Length;
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + cameraOffsets[currentOffsetIndex];
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Mantém um ângulo agradável
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
    }
}
