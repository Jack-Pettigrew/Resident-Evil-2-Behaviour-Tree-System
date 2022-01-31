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

        // Temp FOV variables (move to blackboard visual editor)
        [Header("Field of View")]
        [SerializeField] private float fovAngle = 90.0f;
        [SerializeField] private float fovRange = 5.0f;
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private LayerMask environmentLayerMask;

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

            behaviourTree.Blackboard.AddToBlackboard("TargetDoorPathIndex", 0);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoorPath", null);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoor", null);
            behaviourTree.Blackboard.AddToBlackboard("TargetRoom", null);

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

            // Go To Door, Open Door, Walk to Exit Point, Go to Player
            Selector baseTest = new Selector(behaviourTree, new List<Node> {
                new Sequence(behaviourTree, new List<Node>{
                    // Idle
                    new IsAtTarget<Component>(behaviourTree, "Player", 1.5f),
                    new IdleNode(behaviourTree, "IdleTimerLength")
                }),
                new Sequence(behaviourTree, new List<Node>{
                    // Same room Move to
                    new IsInSameRoomAs<Component>(behaviourTree, "Player"),
                    new MoveTo<Component>(behaviourTree, "Player"),
                }),
                new Selector(behaviourTree, new List<Node> {
                    // new Sequence
                        // IsDoorPathStale (does the door path lead to the same room as Target?)
                        // Get Door Path to Target (make sure this node reset the index)
                        // Update current Move Target
                    new Sequence(behaviourTree, new List<Node>{
                        // Room Path check?
                        new Invertor(behaviourTree,
                            new HasPathTo<Component>(behaviourTree, "TargetDoorPath", "Player")
                        ),
                        new GetDoorPathTo<Component>(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "Player"),
                        new GetDoorFromPath(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor")
                    }),
                    // new Sequence
                        // Repeater: MoveTo TargetDoor until IsAtTarget
                        // Open Door
                        // new GetDoorEntryExitPointNode(behaviourTree, false, "TargetDoor", "MoveTarget"),
                        // new Repeater(behaviourTree, new MoveToNode<Component>(behaviourTree, "MoveTarget"), new IsAtTargetNode<Component>(behaviourTree, "MoveTarget", 1.0f), NodeState.SUCCESSFUL),
                        // increment door index
                    new Sequence(behaviourTree, new List<Node>{
                            // Follow path, use door, increment
                            new GetDoorEntryExitPoint(behaviourTree, true, "TargetDoor", "MoveTarget"),
                            new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 1.0f), NodeState.SUCCESSFUL),
                            new OpenDoor(behaviourTree, "TargetDoor"),
                            new GetDoorEntryExitPoint(behaviourTree, false, "TargetDoor", "MoveTarget"),
                            new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 1.0f), NodeState.SUCCESSFUL),
                            new IncrementDoorPathIndex(behaviourTree, "TargetDoorPathIndex", "TargetDoorPath"),
                            new GetDoorFromPath(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor")
                    })
                })
            });

            /*
             * Final repeater to Player doesn't reset Sequence due to IsAtTarget node in Selector is true before MoveTo<Player> repeater can return success
             * Sequence index needs resetting when the execution branch has changed - need a way to do this (Interuptable? etc)
            */

            return baseTest;
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