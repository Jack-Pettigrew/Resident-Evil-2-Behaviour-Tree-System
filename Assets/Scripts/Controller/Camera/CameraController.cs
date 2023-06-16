using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace DD.Core.Control
{
    public class CameraController : MonoBehaviour
    {
        // Components
        [SerializeField] private InputManager inputManager;

        [Header("Camera Variables")]
        private float pitch, yaw;
        [SerializeField] private float sensitivity = 1.0f;

        [Header("Cinemachine Cameras")]
        [SerializeField] private CinemachineVirtualCamera locomotionCamera;
        [SerializeField] private CinemachineVirtualCamera aimingCamera;

        private void Awake() {
            if(!inputManager)
            {
                inputManager = FindObjectOfType<InputManager>();
                if(!inputManager)
                {
                    Debug.LogError("No Input Manager found.");
                }
            }
        }

        private void LateUpdate()
        {
            // Camera input
            yaw += inputManager.pitchYaw.x * sensitivity;
            pitch += inputManager.pitchYaw.y * sensitivity;

            // Virtual camera swapping
            if (inputManager.Aim && !aimingCamera.gameObject.activeInHierarchy)
            {
                aimingCamera.gameObject.SetActive(true);
            }
            else if (!inputManager.Aim && aimingCamera.gameObject.activeInHierarchy)
            {
                aimingCamera.gameObject.SetActive(false);
            }

            // Virtual camera orbiting
            if (inputManager.Aim)
            {
                aimingCamera.transform.eulerAngles = new Vector3(-pitch, yaw);
            }
            else
            {
                locomotionCamera.transform.eulerAngles = new Vector3(-pitch, yaw);
            }
        }
    }
}