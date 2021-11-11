using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DD.AI.BehaviourTreeSystem;

namespace DD.AI.Controllers
{
    public class AIBeahviourTreeController : MonoBehaviour, IAIBehaviour
    {
        #region AI Events
        public Action<Vector3> MoveEvent { get; set; }
        #endregion

        private BehaviourTree behaviourTree = null;

        // AI MOVEMENT
        [SerializeField] private float moveSpeed = 2.0f;
        private Vector3 velocity = Vector3.zero;
        private float yVelocity = 0;
        [SerializeField] private float groundedGravity = -0.2f;
        [SerializeField] private float gravity = Physics.gravity.y;

        [SerializeField] private float rotSpeedScalar = 0.5f;
        private float currentRotVel = 0;

        // FOV
        [SerializeField] private float fovAngle = 90.0f;
        [SerializeField] private float fovRange = 5.0f;
        [SerializeField] private LayerMask playerLayerMask;

        // COMPONENTS
        private Animator ani = null;
        private CharacterController controller = null;

        private void OnEnable()
        {
            MoveEvent += Move;
        }

        private void OnDisable()
        {
            MoveEvent -= Move;
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            ani = GetComponent<Animator>();
            behaviourTree = new BehaviourTree(this);

            // init Behaviour Tree
            behaviourTree.SetBehaviourTree(CreateBehaviourTree());
        }

        private Node CreateBehaviourTree()
        {
            // Create and add inital variables to BB (to be defined in editor... but made here because custom tools are hard to make lol)
            behaviourTree.Blackboard.AddToBlackboard("Player", FindObjectOfType<Core.Control.PlayerController>().transform);

            /* Create Example BT structure
             Idle
             Search
             Chase
            */

            IdleNode idle = new IdleNode(behaviourTree, 0.0f);
            CanSeePlayerNode canSeePlayer = new CanSeePlayerNode(behaviourTree, fovAngle, fovRange, playerLayerMask);
            MoveToNode moveToPlayer = new MoveToNode(behaviourTree, "Player", 1.5f);
            
            Sequence idleSequence = new Sequence(behaviourTree, new List<Node> { new IsAtTargetNode(behaviourTree), idle });
            Sequence followSequence = new Sequence(behaviourTree, new List<Node> { canSeePlayer, moveToPlayer });

            Selector root = new Selector(behaviourTree, new List<Node> {idleSequence, followSequence, idle});

            return root;
        }

        private void Update()
        {
            behaviourTree.EvaluateTree();

            UpdatePhysics();

            // Anims
            ani.SetFloat("Speed", Mathf.Clamp01(new Vector3(velocity.x, 0, velocity.z).magnitude));

            // Reset after setting Anim variables so we stop moving (workaround)
            velocity = Vector3.zero + Vector3.up * yVelocity;
        }

        private void UpdatePhysics()
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
        }

        // TODO: Decide whether we should pass the Target position instead of just a direction
        //          - would allow for movement smoothing to be handled directly by the actual Move function.
        public Transform GetAITransform()
        {
            return transform;
        }

        public Animator GetAnimator()
        {
            return ani;
        }

        private void Move(Vector3 dir)
        {
            velocity = transform.forward * moveSpeed;

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotVel, rotSpeedScalar);
        }
    }
}