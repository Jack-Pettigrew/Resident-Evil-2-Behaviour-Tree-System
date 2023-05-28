using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DD.AI.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class AILocomotion : MonoBehaviour, IAIMoveable
    {
        // COMPONENTS
        private CharacterController controller;
        private Animator animator;

        // VELOCITY
        private Vector3 velocity = Vector3.zero;
        public Vector3 Velocity { get { return velocity; } }
        private float yVelocity = 0;

        // MOVEMENT
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private float rotSpeedScalar = 0.5f;
        private float currentRotVel = 0;

        // GRAVITY
        [SerializeField] private float groundedGravity = -0.2f;
        [SerializeField] private float gravity = Physics.gravity.y;

        // NAVIGATION
        private NavMeshPath path;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<AIAnimator>().Animator;

            path = new NavMeshPath();
        }

        /// <summary>
        /// Updates the locomotion movement for this AI.
        /// </summary>
        private void Update()
        {
            // PHYSICS
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


            // Reset after setting Anim variables so AI stops playing walk anim (workaround)
            velocity = Vector3.zero + Vector3.up * yVelocity;
            animator.SetFloat("Speed", Mathf.Clamp01(new Vector3(velocity.x, 0, velocity.z).magnitude));
        }

        public bool UpdatePath(Vector3 goalPosition)
        {
            // Update Path
            NavMesh.CalculatePath(transform.position, goalPosition, NavMesh.AllAreas, path);

            // Fail if path is invalid
            if (path.corners.Length <= 0)
            {
                return false;
            }

#if UNITY_EDITOR
            for (int i = 0; i < path.corners.Length; i++)
            {
                if (i + 1 == path.corners.Length) break;

                Debug.DrawLine(path.corners[i], path.corners[i + 1]);
            }
#endif

            return true;
        }

        public void Move()
        {
            if (path.corners.Length > 0)
            {
                Vector3 targetDirection = path.corners[1] - transform.position;

                float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotVel, rotSpeedScalar);

                velocity = transform.forward * moveSpeed;

                animator.SetFloat("Speed", Mathf.Clamp01(new Vector3(velocity.x, 0, velocity.z).magnitude));
            }
        }
    }

}