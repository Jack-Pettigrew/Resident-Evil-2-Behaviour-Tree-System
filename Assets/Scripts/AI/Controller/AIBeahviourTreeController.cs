using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DD.AI.BehaviourTreeSystem;

namespace DD.AI.Controllers
{
    public class AIBeahviourTreeController : MonoBehaviour
    {
        private BehaviourTree behaviourTree = null;

        // Animation
        private Animator ani = null;

        // Locomotion
        private float currentRotVel = 0;
        private Transform agentTarget;

        // FOV (seperate component that updates tree via event)
        [SerializeField] private LayerMask playerLayerMask;

        // COMPONENTS
        private NavMeshAgent agent;

        void Awake()
        {
            ani = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

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
            Sequence root = new Sequence(new List<Node> { new SetAIDestination("Player", SetNavAgentTarget), new MoveToAIDestination(MoveTowardsTarget) });
            behaviourTree.SetBehaviourTree(root);
        }

        void Update()
        {
            behaviourTree.EvaluateTree();

            ani.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
        }

        public void Move(Vector3 dir)
        {
            dir = dir.normalized;
            Vector3 vel = dir * 1.0f;
            transform.position += vel * Time.deltaTime;

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotVel, 1.0f);
        }

        private void SetNavAgentTarget(Transform target)
        {
            agentTarget = target;
        }
        
        private bool MoveTowardsTarget()
        {
            if (agent.isStopped)
                agent.isStopped = false;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(agentTarget.position, path);
            agent.SetPath(path);

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                agent.isStopped = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}