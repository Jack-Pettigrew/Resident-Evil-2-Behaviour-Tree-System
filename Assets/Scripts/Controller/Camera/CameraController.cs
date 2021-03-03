using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float pitch, yaw;
    [SerializeField] private float sensitivity = 1.0f;

    [SerializeField] private Transform viewTarget = null;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch += Input.GetAxis("Mouse Y") * sensitivity;

        transform.eulerAngles = new Vector3(-pitch, yaw);

        transform.position = viewTarget.position - (transform.forward * targetOffset.z) + (transform.up * targetOffset.y) + (transform.right * targetOffset.x);
    }
}
