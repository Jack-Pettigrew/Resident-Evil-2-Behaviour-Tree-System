using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

namespace DD.AI.Controllers
{
    public class AIBeahviourTreeController : MonoBehaviour, IAIBehaviour
    {
        #region AI Events
        public Action<Vector3> MoveEvent { get; set; }
        #endregion

        // BEHAVIOUR TREE
        private BehaviourTree behaviourTree;

        // COMPONENTS - the AI's 'controller'
        private AILocomotion locomotion;

        // FOV
        [SerializeField] private float fovAngle = 90.0f;
        [SerializeField] private float fovRange = 5.0f;
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private LayerMask environmentLayerMask;

        public DD.Systems.Room.Door testDoorTarget;

        private void OnEnable()
        {
            MoveEvent += locomotion.Move;
        }

        private void OnDisable()
        {
            MoveEvent -= locomotion.Move;
        }

        private void Awake()
        {
            locomotion = GetComponent<AILocomotion>();

            behaviourTree = new BehaviourTree(this);
            behaviourTree.SetBehaviourTree(CreateBehaviourTree());
        }

        private Node CreateBehaviourTree()
        {
            // Create and add BB variables (to be defined in editor... but made here because custom Node tools are hard to make lol)
            behaviourTree.Blackboard.AddToBlackboard("Player", FindObjectOfType<Core.Control.PlayerController>());

            behaviourTree.Blackboard.AddToBlackboard("TargetDoor", testDoorTarget);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoorIndex", 0);
            behaviourTree.Blackboard.AddToBlackboard("DoorPathArray", new List<DD.Systems.Room.Door>());

            behaviourTree.Blackboard.AddToBlackboard("IdleTimerLength", 0.0f);
            behaviourTree.Blackboard.AddToBlackboard("fovAngle", fovAngle);
            behaviourTree.Blackboard.AddToBlackboard("fovRange", fovRange);
            behaviourTree.Blackboard.AddToBlackboard("PlayerLayerMask", playerLayerMask);
            behaviourTree.Blackboard.AddToBlackboard("EnvironmentLayerMask", environmentLayerMask);

            /* Create Example BT structure
             Idle
             Search
             Chase
            */

            IdleNode idle = new IdleNode(behaviourTree, "IdleTimerLength");
            //CanSeePlayerNode canSeePlayer = new CanSeePlayerNode(behaviourTree, "fovAngle", "fovRange", "PlayerLayerMask", "EnvironmentLayerMask");
            //MoveToNode moveToPlayer = new MoveToNode(behaviourTree, "Player");

            Sequence idleSequence = new Sequence(behaviourTree, new List<Node> { new IsAtTargetNode<Systems.Room.Door>(behaviourTree, "TargetDoor"), idle });
            //Sequence followSequence = new Sequence(behaviourTree, new List<Node> { canSeePlayer, moveToPlayer });

            //Selector root = new Selector(behaviourTree, new List<Node> { idleSequence, followSequence, idle });

            GoToDoorNode goToDoor = new GoToDoorNode(behaviourTree, "TargetDoor");
            Selector root = new Selector(behaviourTree, new List<Node> { idleSequence, goToDoor });
            return goToDoor;
        }

        private void Update()
        {
            behaviourTree.EvaluateTree();

            locomotion.UpdateLocomotion();
        }


        public Transform GetAITransform()
        {
            return transform;
        }

        public Animator GetAnimator()
        {
            return GetComponent<Animator>();
        }
    }
}