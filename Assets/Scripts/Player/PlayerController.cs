using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Control
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool ignoreInput = false;

        private CharacterController controller = null;

        [Header("Locomotion")]
        [SerializeField] private float walkSpeed = 1.0f;
        [SerializeField] private float runSpeed = 2.0f;
        [SerializeField] private float crouchSpeed = 0.5f;

        private bool isSprinting = false;
        private Vector3 inputDir = Vector3.zero;
        private Vector3 velocity = Vector3.zero;
        private float yVelocity = 0.0f;

        [SerializeField] private float groundedGravity = -0.2f;
        [SerializeField] private float gravity = Physics.gravity.y;

        [SerializeField] private Transform cameraTransform = null;
        [SerializeField] private float turnSpeedScalar = 0.5f;
        private float turnSmoothingVar = 0.0f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            inputDir =  ignoreInput ? Vector3.zero : new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if(inputDir.sqrMagnitude > 0)
            {
                float targetAngle;

                if (!isSprinting) // STRAFE (Camera Forward)
                {
                    targetAngle = cameraTransform.eulerAngles.y;
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVar, turnSpeedScalar);

                    inputDir = transform.rotation * inputDir;
                }
                else    // Input Forward
                {

                }
            }

            // Gravity
            if (controller.isGrounded)
            {
                yVelocity = groundedGravity;
            }
            else
            {
                yVelocity += gravity * Time.deltaTime;
            }

            // Move
            velocity = (inputDir * inputDir.magnitude * walkSpeed) + (Vector3.up * yVelocity);
            controller.Move(velocity * Time.deltaTime);
        }
    }
}