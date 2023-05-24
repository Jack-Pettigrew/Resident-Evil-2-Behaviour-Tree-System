using System;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.AI.BehaviourTreeSystem;
using DD.AI.States;

namespace DD.AI.Controllers
{
    public class AIBehaviourTreeController : MonoBehaviour, IAIBehaviour
    {
        // BEHAVIOUR TREE
        private BehaviourTree behaviourTree;

        // COMPONENTS - the AI's 'controller'
        private AILocomotion aiLocomotion;
        private AIAnimator aiAnimator;
        [SerializeField] private Interactor interactor;

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
            behaviourTree.Blackboard.AddToBlackboard("Player", FindObjectOfType<PlayerController>());
            behaviourTree.Blackboard.AddToBlackboard("State", MrXState.SEARCHING);
            behaviourTree.Blackboard.AddToBlackboard("MoveTarget", null);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoorPathIndex", 0);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoorPath", null);
            behaviourTree.Blackboard.AddToBlackboard("TargetDoor", null);
            behaviourTree.Blackboard.AddToBlackboard("TargetRoom", null);
            behaviourTree.Blackboard.AddToBlackboard("IdleTimerLength", 5.0f);
            behaviourTree.Blackboard.AddToBlackboard("LastKnownLocation", Vector3.zero);
            behaviourTree.Blackboard.AddToBlackboard("TargetSearchRoom", null);
            behaviourTree.Blackboard.AddToBlackboard("SearchRoomCounter", 0);

            // Creating actual behaviour tree
            Selector root = new Selector(behaviourTree,
                new List<Node> {
                    // ! Attack
                    new ConditionalBranch(behaviourTree, new CompareBlackboardVariable<MrXState>(behaviourTree, MrXState.ATTACKING, "State"),
                        new Selector(behaviourTree, new List<Node> {
                            
                            // ! Attacking Player + Move to Player
                            new Selector(behaviourTree, new List<Node> {

                                // Atttacking Player
                                new Sequence(behaviourTree, new List<Node> {
                                    new IsAtTarget<Component>(behaviourTree, "Player", 1.0f),
                                    new PlayAnimation(behaviourTree, "right_hook", true)
                                }),                                

                                // Log LKL + Move To Player
                                new Sequence(behaviourTree, new List<Node> {
                                    new CanSeeObject(behaviourTree),
                                    new SetLastKnownLocation(behaviourTree, "LastKnownLocation", "Player"),                                    
                                    new IsInSameRoomAs<Component>(behaviourTree, "Player"),
                                    new MoveTo<Component>(behaviourTree, "Player")
                                }),
                            }),

                            // ! LKL
                            new Selector(behaviourTree, new List<Node> {
                                // End Search
                                new Sequence(behaviourTree, new List<Node> {
                                    new IsAtPoint(behaviourTree, "LastKnownLocation", 1.0f),
                                    new PlayAnimation(behaviourTree, "looking_around", true),
                                    new SetBlackboardVariable<MrXState>(behaviourTree, "State", MrXState.SEARCHING)
                                }),

                                // Go to LKL directly
                                new Sequence(behaviourTree, new List<Node> {
                                    new IsInSameRoomAsVector3(behaviourTree, "LastKnownLocation"),
                                    new MoveToVector3(behaviourTree, "LastKnownLocation")
                                }),

                                // Go to LKL via Rooms
                                new Sequence(behaviourTree, new List<Node> {
                                    // Room Transition
                                    new FindDoorPathToVector3(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "LastKnownLocation"),
                                    new GetDoorFromPath(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor"),
                                    new GetDoorEntryExitPoint(behaviourTree, true, "TargetDoor", "MoveTarget"),
                                    new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),

                                    // Room Transition
                                    new Selector(behaviourTree, new List<Node> {
                                        // Locked Door?
                                        new Sequence(behaviourTree, new List<Node> {
                                            new Invertor(behaviourTree, new CanUseDoor(behaviourTree, "TargetDoor")),
                                            new CloseDoor(behaviourTree, "TargetDoor"),
                                            new BangDoor(behaviourTree, "TargetDoor"),
                                            new IdleNode(behaviourTree, "IdleTimerLength"),
                                            new GetRandomRoom(behaviourTree, "TargetSearchRoom"),
                                            new SetBlackboardVariable<MrXState>(behaviourTree, "State", MrXState.SEARCHING)
                                        }),

                                        // Use Door
                                        new Sequence(behaviourTree, new List<Node> {
                                            new OpenDoor(behaviourTree, "TargetDoor"),
                                            new SendAnimationRigSignal(behaviourTree, "door", Animation.RigEvents.AnimRigEventType.ENABLE),
                                            new GetDoorEntryExitPoint(behaviourTree, false, "TargetDoor", "MoveTarget"),
                                            new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                                            new SendAnimationRigSignal(behaviourTree, "door", Animation.RigEvents.AnimRigEventType.DISABLE),
                                            new IncrementDoorPathIndex(behaviourTree, "TargetDoorPathIndex", "TargetDoorPath"),
                                        }, true)
                                    })


                                })
                            })

                        })
                    ),

                    // ! Search
                    new ConditionalBranch(behaviourTree, new CompareBlackboardVariable<MrXState>(behaviourTree, MrXState.SEARCHING, "State"),
                        new Selector(behaviourTree, new List<Node> {

                            // ! Init Search
                            new Sequence(behaviourTree, new List<Node> {
                                new IsBlackboardVariableNull(behaviourTree, "TargetSearchRoom"),
                                new GetRandomRoomAdjacentToTarget(behaviourTree, true, "Player", "TargetSearchRoom")
                            }),

                            // ! Can See Player?
                            new Sequence(behaviourTree, new List<Node> {
                                new CanSeeObject(behaviourTree),
                                new SetLastKnownLocation(behaviourTree, "LastKnownLocation", "Player"),
                                new SetBlackboardVariable<MrXState>(behaviourTree, "State", MrXState.ATTACKING)
                            }),

                            // ! Search Room + Go to Room
                            new Selector(behaviourTree, new List<Node> {
                                // Search Room
                                new Sequence(behaviourTree, new List<Node> {
                                    new IsInRoom(behaviourTree, "TargetSearchRoom"),
                                    new Repeater(behaviourTree,
                                        new Sequence(behaviourTree, new List<Node> {
                                            new GetRandomRoomSearchSpot(behaviourTree, "TargetSearchRoom", "MoveTarget"),
                                            new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                                            new PlayAnimation(behaviourTree, "looking_around", true),
                                            new IncrementBlackboardVariable(behaviourTree, "SearchRoomCounter", 1)
                                        }),
                                        new CompareBlackboardVariable<int>(behaviourTree, 2, "SearchRoomCounter"),
                                        NodeState.SUCCESSFUL
                                    ),
                                    new SetBlackboardVariable<int>(behaviourTree, "SearchRoomCounter", 0),
                                    new GetRandomRoomAdjacentToTarget(behaviourTree, true, "Player", "TargetSearchRoom")
                                }),
                                
                                // Room Transition
                                new Sequence(behaviourTree, new List<Node> {
                                    new FindDoorPathTo<Component>(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetSearchRoom"),
                                    new GetDoorFromPath(behaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor"),
                                    new GetDoorEntryExitPoint(behaviourTree, true, "TargetDoor", "MoveTarget"),
                                    new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),

                                    new Selector(behaviourTree, new List<Node> {
                                        // Locked Door?
                                        new Sequence(behaviourTree, new List<Node> {
                                            new Invertor(behaviourTree, new CanUseDoor(behaviourTree, "TargetDoor")),
                                            new CloseDoor(behaviourTree, "TargetDoor"),
                                            new BangDoor(behaviourTree, "TargetDoor"),
                                            new IdleNode(behaviourTree, "IdleTimerLength"),
                                            new GetRandomRoom(behaviourTree, "TargetSearchRoom")
                                        }),

                                        // Use Door
                                        new Sequence(behaviourTree, new List<Node> {
                                            new OpenDoor(behaviourTree, "TargetDoor"),
                                            new SendAnimationRigSignal(behaviourTree, "door", Animation.RigEvents.AnimRigEventType.ENABLE),
                                            new GetDoorEntryExitPoint(behaviourTree, false, "TargetDoor", "MoveTarget"),
                                            new Repeater(behaviourTree, new MoveTo<Component>(behaviourTree, "MoveTarget"), new IsAtTarget<Component>(behaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                                            new SendAnimationRigSignal(behaviourTree, "door", Animation.RigEvents.AnimRigEventType.DISABLE),
                                            new IncrementDoorPathIndex(behaviourTree, "TargetDoorPathIndex", "TargetDoorPath"),
                                        }, true)
                                    })


                                })
                            })
                        })
                    ),

                    // ! Idle
                    new IdleNode(behaviourTree)
                }
            );

            return root;
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

        public Interactor GetInteractor()
        {
            return interactor ?? null;
        }

        public T GetAIComponent<T>()
        {
            T component = GetComponent<T>();

            if (component == null)
            {
                component = GetComponentInChildren<T>(true);
            }

            return component;
        }
    }
}