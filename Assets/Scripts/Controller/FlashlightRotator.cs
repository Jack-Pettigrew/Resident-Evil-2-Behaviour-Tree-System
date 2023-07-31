using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DD.Core.Control;

public class FlashlightRotator : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineVirtualCamera locomotionCamera;
    [SerializeField] private CinemachineVirtualCamera aimingCamera;
    [SerializeField] private float slerpSpeed = 1.0f;

    private void LateUpdate()
    {
        CinemachineVirtualCamera handleCamera = InputManager.Instance.Aim ? aimingCamera : locomotionCamera;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, handleCamera.transform.rotation, Time.deltaTime * slerpSpeed);
    }
}
