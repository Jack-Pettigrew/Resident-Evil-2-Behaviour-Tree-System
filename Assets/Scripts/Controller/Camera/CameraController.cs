using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Variables")]
    private float pitch, yaw;
    [SerializeField] private float sensitivity = 1.0f;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineVirtualCamera locomotionCamera;
    [SerializeField] private CinemachineVirtualCamera aimingCamera;

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch += Input.GetAxis("Mouse Y") * sensitivity;

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            aimingCamera.gameObject.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            aimingCamera.gameObject.SetActive(false);
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            aimingCamera.transform.eulerAngles = new Vector3(-pitch, yaw);
        }
        else
        {
            locomotionCamera.transform.eulerAngles = new Vector3(-pitch, yaw);
        }
    }
}
