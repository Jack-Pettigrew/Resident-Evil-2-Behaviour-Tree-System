using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DD.Core.Control
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private InputManager inputManager;
        private CharacterController controller = null;

        [Header("Global")]
        private Vector3 velocity = Vector3.zero;
        private float yVelocity = 0.0f;

        [Header("Locomotion")]
        [SerializeField] private float gravity = Physics.gravity.y;
        [SerializeField] private float groundedGravity = -0.2f;
        [SerializeField] private float walkSpeed = 1.0f;
        [SerializeField] private float runSpeed = 2.0f;
        [SerializeField] private float locomotionTurnSpeedScalar = 0.5f;
        private float turnSmoothingVar = 0.0f;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform = null;

        [Header("Animation")]
        [SerializeField] private Animator animator = null;
        [SerializeField] private MultiAimConstraint aimRigConstraint;

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

            if(!inputManager)
            {
                inputManager = FindObjectOfType<InputManager>();
                if(!inputManager)
                {
                    Debug.LogError("No Input Manager found.");
                }
            }
        }

        void Update()
        {
            Turn();

            Move();
        }

        private void LateUpdate() {
            UpdateAnimations();
        }

        /// <summary>
        /// Turns Player according to movement type (Strafe or Input Direction).
        /// </summary>
        private void Turn()
        {
            // Aim Turning
            if(inputManager.Aim)
            {
                transform.eulerAngles = Vector3.up * cameraTransform.eulerAngles.y;
                return;
            }
            
            // Locomotion Turning
            if (inputManager.InputDirection.sqrMagnitude > 0)
            {
                float targetAngle;

                // STRAFE (Camera Forward)
                if (!inputManager.Sprint)
                {
                    targetAngle = cameraTransform.eulerAngles.y;
                }
                // Input Forward
                else
                {
                    targetAngle = Mathf.Atan2(inputManager.InputDirection.x, inputManager.InputDirection.z) * Mathf.Rad2Deg + cameraTransform.localEulerAngles.y;
                }

                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVar, locomotionTurnSpeedScalar);
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

            // Move
            if (!inputManager.Sprint) // Strafing
            {
                velocity = ((transform.rotation * inputManager.InputDirection) * inputManager.InputDirection.magnitude * walkSpeed) + (Vector3.up * yVelocity);
                controller.Move(velocity * Time.deltaTime);
            }
            else // Input Forward Based
            {
                velocity = (transform.forward * inputManager.InputDirection.magnitude * runSpeed) + (Vector3.up * yVelocity);
                controller.Move(velocity * Time.deltaTime);
            }
        }

        private void UpdateAnimations()
        {
            // Locomotion
            animator.SetFloat("VelX", Mathf.Lerp(animator.GetFloat("VelX"), inputManager.InputDirection.x, Time.deltaTime * 10.0f));
            animator.SetFloat("VelY", Mathf.Lerp(animator.GetFloat("VelY"), inputManager.InputDirection.z, Time.deltaTime * 10.0f));
            animator.SetBool("isSprinting", inputManager.Sprint);

            // Aiming
            animator.SetLayerWeight(1, (inputManager.Aim ? 1.0f : 0.0f));
            aimRigConstraint.weight = inputManager.Aim ? 1.0f : 0.0f;
        }
    }
}