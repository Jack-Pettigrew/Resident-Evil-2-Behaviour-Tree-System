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
            CinemachineVirtualCamera handleCamera = inputManager.Aim ? aimingCamera : locomotionCamera;
            handleCamera.transform.eulerAngles = new Vector3(-pitch, yaw);

            // Handle Camera Colliding
            // CinemachineTransposer transposer = handleCamera.GetCinemachineComponent<CinemachineTransposer>();
            // CinemachineFramingTransposer cinemachineFramingTransposer = handleCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFramingTransposer;
            // Vector3 cinemachineCameraPosition = (handleCamera.Follow.position - new Vector3(0, -transposer.m_FollowOffset.y, cinemachineFramingTransposer.m_CameraDistance));
            // Vector3 cameraTargetDir = handleCamera.Follow.position - cinemachineCameraPosition;
            
            // if(Physics.Raycast(cinemachineCameraPosition, cameraTargetDir, out RaycastHit hit, cameraTargetDir.magnitude))
            // {
            //     transform.position = hit.point;
            // }
        }

        // private void OnDrawGizmosSelected() {
        //     CinemachineVirtualCamera handleCamera = inputManager.Aim ? aimingCamera : locomotionCamera;
        //     CinemachineFramingTransposer transposer = handleCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        //     CinemachineFramingTransposer cinemachineFramingTransposer = handleCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFramingTransposer;
            
        //     Vector3 cameraTargetDir = handleCamera.Follow.position - (handleCamera.Follow.position + new Vector3(0, transposer.m_TrackedObjectOffset.y, cinemachineFramingTransposer.m_CameraDistance));

        //     Debug.DrawLine(handleCamera.Follow.position, (handleCamera.Follow.position - new Vector3(0, -transposer.m_TrackedObjectOffset.y, cinemachineFramingTransposer.m_CameraDistance)), Color.white);
        // }
    }
}