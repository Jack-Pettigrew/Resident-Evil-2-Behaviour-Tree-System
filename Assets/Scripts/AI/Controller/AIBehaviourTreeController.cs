using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;
using DD.Animation.RigEvents;

namespace DD.AI.Controllers
{
    public class AIBehaviourTreeController : MonoBehaviour, IAIBehaviour
    {
        // BEHAVIOUR TREE
        private BehaviourTree behaviourTree;

        // COMPONENTS - the AI's 'controller'
        private AILocomotion aiLocomotion;
        private AIAnimator aiAnimator;

        private void Awake()
        {
            aiLocomotion = GetComponent<AILocomotion>();
            aiAnimator = GetComponent<AIAnimator>();

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

            /* Create Example BT structure
             Idle
             Search
             Chase
            */

            // Go To Door, Open Door, Walk to Exit Point, Go to Player
            Selector baseTest = new Selector(behaviourTree, new List<Node> {                
                                
                // Attack
                new Sequence(behaviourTree, new List<Node>{
                    new IsAtTarget<Component>(behaviourTree, "Player", 1.5f),
                    new PlayAnimation(behaviourTree, "Right Hook", true)
                }),

                // Move to Player
                new Sequence(behaviourTree, new List<Node>{
                    // Same room Move to
                    new IsInSameRoomAs<Component>(behaviourTree, "Player"),
                    new MoveTo<Component>(behaviourTree, "Player"),
                }),
                    
                // Find Path if doesn't have one
                new Sequence(behaviourTree, new List<Node> {
                    // Invert to trigger find door path
                    new Invertor(behaviourTree,
                        new HasRoomPathTo<Component>(behaviourTree, "TargetDoorPath", "Player")
                    ),
                    new FindDoorPathTo<Component>(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "Player")
                }),

                // Follow path via Doors
                new Sequence(behaviourTree, new List<Node>{
                    new GetDoorFromPath(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor"),
                    new GetDoorEntryExitPoint(behaviourTree, true, "TargetDoor", "MoveTarget"),
                    new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                    new OpenDoor(behaviourTree, "TargetDoor"),
                    new SendAnimationRigSignal(behaviourTree, "door", AnimRigEventType.ENABLE),
                    new GetDoorEntryExitPoint(behaviourTree, false, "TargetDoor", "MoveTarget"),
                    new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                    new SendAnimationRigSignal(behaviourTree, "door", AnimRigEventType.DISABLE),
                    new IncrementDoorPathIndex(behaviourTree, "TargetDoorPathIndex", "TargetDoorPath"),
                }, true),

                new IdleNode(behaviourTree)
            });

            return baseTest;
        }

        private void Update()
        {
            behaviourTree.EvaluateTree();
        }


        public Transform GetAITransform()
        {
            return transform;
        }

        public AIAnimator GetAnimator()
        {
            return aiAnimator;
        }

        public T GetAIComponent<T>()
        {
            T component = GetComponent<T>();

            if(component == null)
            {
                component = GetComponentInChildren<T>(true);
            }

            return component;
        }
    }
}