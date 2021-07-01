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
        public Action<Transform> SetMoveTarget { get; set; }
        public Action<Vector3> MoveEvent { get; set; }

        public Func<Transform> GetAITransform { get; set; }
        public Func<Transform> GetAIMoveTarget { get; set; }
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
        public Transform MoveTarget { private set; get; }

        // FOV (seperate component that updates tree via event)
        [SerializeField] private LayerMask playerLayerMask;

        // COMPONENTS
        private Animator ani = null;
        private CharacterController controller = null;

        private void OnEnable()
        {
            MoveEvent += Move;
            GetAITransform += GetTransform;
            GetAIMoveTarget += GetMoveTarget;
        }

        private void OnDisable()
        {
            MoveEvent -= Move;
            GetAITransform -= GetTransform;
            GetAIMoveTarget -= GetMoveTarget;
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            ani = GetComponent<Animator>();
            behaviourTree = new BehaviourTree();

            // Create and add inital variables to BB (usually defined in an editor - but forced here because they're a pain to create)
            //Blackboard.AddToSharedBlackboard("Player", FindObjectOfType<Core.Control.PlayerController>().transform);
            GetAIBlackboard().AddToBlackboard("Player", FindObjectOfType<Core.Control.PlayerController>().transform);

            // Create BT structure
            // Idle
            // Search
            // Chase
            MoveToNode moveToPlayer = new MoveToNode(this, "Player", 1.5f);
            IdleNode idle = new IdleNode();

            // Set Root
            CanSeePlayerNode root = new CanSeePlayerNode(moveToPlayer, idle, this, 90, 10.0f, playerLayerMask);

            //CheckBlackboardVariableNode<Transform> root = new CheckBlackboardVariableNode<Transform>("Player", FindObjectOfType<Core.Control.PlayerController>().transform, ConditionType.Equals, this);
            behaviourTree.SetBehaviourTree(root);
        }

        private void Update()
        {
            behaviourTree.EvaluateTree();

            ApplyPhysics();

            // Anims
            ani.SetFloat("Speed", Mathf.Clamp01(new Vector3(velocity.x, 0, velocity.z).magnitude));

            // Reset after setting Anim variables so we stop moving
            velocity = Vector3.zero + Vector3.up * yVelocity;
        }

        private void ApplyPhysics()
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
        public Transform GetTransform()
        {
            return transform;
        }

        public Blackboard GetAIBlackboard()
        {
            return behaviourTree.Blackboard;
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

        public Transform GetMoveTarget()
        {
            return MoveTarget;
        }
    }
}