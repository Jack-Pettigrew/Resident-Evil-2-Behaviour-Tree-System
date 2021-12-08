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
        public DD.Systems.Room.Room testRoomTarget;

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
            behaviourTree.Blackboard.AddToBlackboard("MoveTarget", null);

            //behaviourTree.Blackboard.AddToBlackboard("TargetDoor", testDoorTarget);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoorPathIndex", 0);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoorPath", null);
            behaviourTree.Blackboard.AddToBlackboard("TargetRoom", testRoomTarget);

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

            //IdleNode idle = new IdleNode(behaviourTree, "IdleTimerLength");
            //Sequence idleSequence = new Sequence(behaviourTree, new List<Node> { new IsAtTargetNode<Component>(behaviourTree, "MoveTarget", 0.5f), idle });

            //IsBlackboardVariableNull isNull = new IsBlackboardVariableNull(behaviourTree, "MoveTarget");
            //GetDoorEntryExitPointNode doorEntryExitPoint = new GetDoorEntryExitPointNode(behaviourTree, true, "TargetDoor", "MoveTarget");

            //Sequence isMoveTargetNullSequence = new Sequence(behaviourTree, new List<Node> { isNull, doorEntryExitPoint });

            //MoveToNode<Transform> goToDoor = new MoveToNode<Transform>(behaviourTree, "MoveTarget");
            //Sequence goToDoorSequence = new Sequence(behaviourTree, new List<Node> { doorEntryExitPoint, goToDoor });

            //Selector root = new Selector(behaviourTree, new List<Node> { isMoveTargetNullSequence, idleSequence, goToDoorSequence });

            //Sequence test = new Sequence(behaviourTree, new List<Node> {
            //    new Repeater(behaviourTree, new MoveToNode<Component>(behaviourTree, "TargetDoor"), new IsAtTargetNode<Component>(behaviourTree, "TargetDoor", 3.0f), NodeState.SUCCESSFUL),
            //    new Repeater(behaviourTree, new MoveToNode<Component>(behaviourTree, "Player"), new IsAtTargetNode<Component>(behaviourTree, "Player", 1.5f), NodeState.SUCCESSFUL )
            //});

            // Go To Door, Open Door, Walk to Exit Point, Go to Player
            Selector baseTest = new Selector(behaviourTree, new List<Node> {
                new Sequence(behaviourTree, new List<Node>{
                    new IsAtTarget<Component>(behaviourTree, "Player", 1.5f),
                    new IdleNode(behaviourTree, "IdleTimerLength")
                }),
                new Selector(behaviourTree, new List<Node>{
                    new IsInSameRoomAs<Component>(behaviourTree, "Player"),
                    new MoveTo<Component>(behaviourTree, "Player"),
                }),
                new Selector(behaviourTree, new List<Node> {
                    new Sequence(behaviourTree, new List<Node>{ 
                        // Has Path to Target Room?
                        new GetDoorPathToRoom(behaviourTree, "TargetDoorPath", "TargetRoom")
                    })
                    // new Sequence
                        // IsDoorPathStale (does the door path lead to the same room as Target?)
                        // Get Door Path to Target (make sure this node reset the index)
                    // new Sequence
                        // Update current Target Door
                        // Repeater: MoveTo TargetDoor until IsAtTarget
                        // Open Door
                        // new GetDoorEntryExitPointNode(behaviourTree, false, "TargetDoor", "MoveTarget"),
                        // new Repeater(behaviourTree, new MoveToNode<Component>(behaviourTree, "MoveTarget"), new IsAtTargetNode<Component>(behaviourTree, "MoveTarget", 1.0f), NodeState.SUCCESSFUL),
                        // increment door index
                })
                //new Sequence(behaviourTree, new List<Node>{
                //    new GetDoorEntryExitPointNode(behaviourTree, true, "TargetDoor", "MoveTarget"),
                //    new Repeater(behaviourTree, new MoveToNode<Component>(behaviourTree, "MoveTarget"), new IsAtTargetNode<Component>(behaviourTree, "MoveTarget", 1.0f), NodeState.SUCCESSFUL),
                //    new OpenDoorNode(behaviourTree, "TargetDoor"),
                //    new GetDoorEntryExitPointNode(behaviourTree, false, "TargetDoor", "MoveTarget"),
                //    new Repeater(behaviourTree, new MoveToNode<Component>(behaviourTree, "MoveTarget"), new IsAtTargetNode<Component>(behaviourTree, "MoveTarget", 1.0f), NodeState.SUCCESSFUL),
                //    new Repeater(behaviourTree, new MoveToNode<Component>(behaviourTree, "Player"), new IsAtTargetNode<Component>(behaviourTree, "Player", 1.5f), NodeState.SUCCESSFUL)
                //})
            });

            /*
             * Final repeater to Player doesn't reset Sequence due to IsAtTarget node in Selector is true before MoveTo<Player> repeater can return success
             * Fix? Make a full "Go to Room > Go to Player" test tree
            */

            return new Sequence(behaviourTree, new List<Node>{
                new Invertor(behaviourTree,
                    new HasPathToRoom(behaviourTree, "TargetDoorPath", "TargetRoom")
                ),
                new GetDoorPathToRoom(behaviourTree, "TargetDoorPath", "TargetRoom"),
            });

            //return baseTest;
            //return test;
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