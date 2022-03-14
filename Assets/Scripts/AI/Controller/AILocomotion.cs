using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class AILocomotion : MonoBehaviour
    {
        // COMPONENTS
        private CharacterController controller;
        private Animator animator;

        // VELOCITY
        [HideInInspector] public Vector3 velocity = Vector3.zero;
        private float yVelocity = 0;

        // MOVEMENT
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private float rotSpeedScalar = 0.5f;
        private float currentRotVel = 0;

        // GRAVITY
        [SerializeField] private float groundedGravity = -0.2f;
        [SerializeField] private float gravity = Physics.gravity.y;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<AIAnimator>().Animator;
        }

        /// <summary>
        /// Updates the locomotion movement for this AI.
        /// </summary>
        public void UpdateLocomotion()
        {
            if (controller.isGrounded)
            {
                yVelocity = groundedGravity;
            }
            else
            {
                yVelocity += gravity * Time.deltaTime;
            }

            velocity.y = yVelocity;

            controller.Move(velocity * Time.deltaTime);

            UpdateAnimator();

            // Reset after setting Anim variables so AI stops playing walk anim (workaround)
            velocity = Vector3.zero + Vector3.up * yVelocity;
        }

        private void UpdateAnimator()
        {
            animator.SetFloat("Speed", Mathf.Clamp01(new Vector3(velocity.x, 0, velocity.z).magnitude));
        }

        /// <summary>
        /// Moves the AI in the direction given.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        public void Move(Vector3 direction)
        {
            velocity = transform.forward * moveSpeed;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotVel, rotSpeedScalar);
        }
    }

}