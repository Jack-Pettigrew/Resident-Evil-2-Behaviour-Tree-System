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

        private void InitAIEvents()
        {
            SetMoveTarget += SetMoveTarget;
            MoveEvent += Move;
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            ani = GetComponent<Animator>();

            behaviourTree = new BehaviourTree();

            // BB: Player Reference
            Blackboard.AddToSharedBlackboard("Player", FindObjectOfType<Core.Control.PlayerController>().transform);

            // BT: Init
            //Sequence followSequence = new Sequence(new List<Node> { new MoveToPlayerNode(this), new JumpNode(this) });
            //CanSeePlayerNode canSeePlayerNode = new CanSeePlayerNode(transform, 45.0f, 2.0f, playerLayerMask);
            //SpinNode spinNode = new SpinNode( transform);
            //Sequence spinSelfSequence = new Sequence( new List<Node> { canSeePlayerNode, spinNode });

            //Selector root = new Selector(new List<Node> { spinSelfSequence, followSequence });

            //SetAIDestination root = new SetAIDestination("Player", SetNavAgentTarget);

            Sequence root = new Sequence(new List<Node> { new SetAIDestination("Player", this), new MoveToAIDestination(this, 2.0f) });
            behaviourTree.SetBehaviourTree(root);
        }

        private void Update()
        {
            behaviourTree.EvaluateTree();

            ApplyPhysics();

            // Anims
            ani.SetFloat("Speed", Mathf.Clamp01(new Vector3(velocity.x, 0, velocity.z).magnitude));
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
            //velocity = Vector3.zero + Vector3.up * yVelocity;

        }

        private void Move(Vector3 dir)
        {
            //dir = dir.normalized;
            //Vector3 vel = dir * 1.0f;
            //transform.position += vel * Time.deltaTime * moveSpeed;

            //transform.position += transform.forward * Time.deltaTime * moveSpeed;

            velocity = transform.forward * moveSpeed;

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotVel, rotSpeedScalar);
        }

        private void SetNavAgentTarget(Transform target)
        {
            MoveTarget = target;
        }
    }
}