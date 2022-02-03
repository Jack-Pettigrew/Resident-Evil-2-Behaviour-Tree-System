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
        [SerializeField] private Animator animator = null;

        [Header("Locomotion")]
        [SerializeField] private float walkSpeed = 1.0f;
        [SerializeField] private float runSpeed = 2.0f;
        [SerializeField] private float turnSpeedScalar = 0.5f;
        private float turnSmoothingVar = 0.0f;

        private bool isSprinting = false;
        private Vector3 inputDir = Vector3.zero;
        private Vector3 velocity = Vector3.zero;
        private float yVelocity = 0.0f;

        [SerializeField] private float groundedGravity = -0.2f;
        [SerializeField] private float gravity = Physics.gravity.y;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform = null;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();

            if(animator == null)
            {
                animator = GetComponentInChildren<Animator>();

                if(animator == null)
                {
                    Debug.LogError($"No Animator found for {name}");
                }
            }
        }

        public void ToggleIgnoreInput(bool toggle) => ignoreInput = toggle;

        void Update()
        {
            inputDir =  ignoreInput ? Vector3.zero : new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            Turn();

            Move();

            UpdateAnimations();
        }

        /// <summary>
        /// Turns Player according to movement type (Strafe or Input Direction).
        /// </summary>
        private void Turn()
        {
            if (inputDir.sqrMagnitude > 0)
            {
                float targetAngle;

                if (!isSprinting) // STRAFE (Camera Forward)
                {
                    targetAngle = cameraTransform.eulerAngles.y;
                }
                else    // Input Forward
                {
                    targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.localEulerAngles.y;
                }

                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVar, turnSpeedScalar);
            }
        }

        /// <summary>
        /// Moves Player according to movement type (strafe or Input Direction).
        /// </summary>
        private void Move()
        {
            // Gravity
            if (controller.isGrounded)
            {
                yVelocity = groundedGravity;
            }
            else
            {
                yVelocity += gravity * Time.deltaTime;
            }

            // Sprint Check
            if(isSprinting && inputDir.sqrMagnitude <= 0)
            {
                isSprinting = false;
            }
            else if(!isSprinting)
            {
                isSprinting = Input.GetKeyDown(KeyCode.LeftShift);
            }

            // Move
            if (!isSprinting) // Strafing
            {
                velocity = ((transform.rotation * inputDir) * inputDir.magnitude * walkSpeed) + (Vector3.up * yVelocity);
                controller.Move(velocity * Time.deltaTime);
            }
            else // Input Forward Based
            {
                velocity = (transform.forward * inputDir.magnitude * runSpeed) + (Vector3.up * yVelocity);
                controller.Move(velocity * Time.deltaTime);
            }
        }

        private void UpdateAnimations()
        {
            animator.SetFloat("VelX", inputDir.x);
            animator.SetFloat("VelY", inputDir.z);
            animator.SetBool("isSprinting", isSprinting);
        }
    }

}