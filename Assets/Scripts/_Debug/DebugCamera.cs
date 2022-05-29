using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using Cinemachine;
using UnityEngine.Animations.Rigging;

public class DebugCamera : MonoBehaviour
{
    private bool active = false;
    [SerializeField] private float speed = 0.02f;
    [SerializeField] private float mouseSenistivity = 0.02f;
    [SerializeField] private bool frozenTime = false;

    private Camera debugCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerObject;

    // Camera
    private float pitch = 0;
    private float yaw = 0;

    private void Awake() {
        debugCamera = GetComponent<Camera>();
        debugCamera.enabled = false;
        debugCamera.GetComponent<AudioListener>().enabled = active;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ToggleDebugCamera();
        }

        if(active)
        {            
            if(Input.GetKeyDown(KeyCode.O))
            {
                PauseTime();
            }

            float verticalMovement = Input.GetKey(KeyCode.Q) ? -1 : Input.GetKey(KeyCode.E) ? 1 : 0;
            
            transform.position += debugCamera.transform.rotation * new Vector3(Input.GetAxisRaw("Horizontal"), verticalMovement, Input.GetAxisRaw("Vertical")) * speed;

            speed = Mathf.Max(0, speed += (Input.mouseScrollDelta.y * 0.01f));

            pitch = Mathf.Clamp(pitch += -Input.GetAxis("Mouse Y") * mouseSenistivity, -90, 90);
            yaw = yaw += Input.GetAxis("Mouse X") * mouseSenistivity;

            transform.localEulerAngles = new Vector3(pitch, yaw);
        }
    }

    private void ToggleDebugCamera()
    {
        active = !active;
        InputManager.Instance.ToggleIgnoreInput(active);

        // Main Camera
        mainCamera.gameObject.SetActive(!active);
        mainCamera.GetComponent<AudioListener>().enabled = !active;

        // Toggle position
        debugCamera.enabled = active;
        transform.position = playerObject.transform.position;
        GetComponent<AudioListener>().enabled = active;
    }

    private void PauseTime()
    {
        frozenTime = !frozenTime;
        Time.timeScale = frozenTime ? 0 : 1;
    }
}