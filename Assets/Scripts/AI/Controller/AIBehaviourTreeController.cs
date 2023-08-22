using System;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.AI.BehaviourTreeSystem;
using DD.AI.States;

namespace DD.AI.Controllers
{
    public class AIBehaviourTreeController : MonoBehaviour, IAIBehaviour, IBehaviourTreeInspectable
    {
        // BEHAVIOUR TREE
        public BehaviourTree BehaviourTree { private set; get; }

        // COMPONENTS - the AI's 'controller'
        private AILocomotion aiLocomotion;
        private AIAnimator aiAnimator;
        [SerializeField] private Interactor interactor;

        private void Awake()
        {
            aiLocomotion = GetComponent<AILocomotion>();
            aiAnimator = GetComponent<AIAnimator>();

            BehaviourTree = new BehaviourTree(this);
            BehaviourTree.SetBehaviourTree(CreateBehaviourTree());
        }

        private Node CreateBehaviourTree()
        {
            // Create and add BB variables (to be defined in editor... but made here because custom Node tools are hard to make lol)
            BehaviourTree.Blackboard.AddToBlackboard("Player", FindObjectOfType<PlayerController>());
            BehaviourTree.Blackboard.AddToBlackboard("State", MrXState.SEARCHING);
            BehaviourTree.Blackboard.AddToBlackboard("MoveTarget", null);
            BehaviourTree.Blackboard.AddToBlackboard("TargetDoorPathIndex", 0);
            BehaviourTree.Blackboard.AddToBlackboard("TargetDoorPath", null);
            BehaviourTree.Blackboard.AddToBlackboard("TargetDoor", null);
            BehaviourTree.Blackboard.AddToBlackboard("IdleTimerLength", 5.0f);
            // BehaviourTree.Blackboard.AddToBlackboard("LastKnownLocation", Vector3.zero);     // Commented out due to attach tree change
            BehaviourTree.Blackboard.AddToBlackboard("TargetSearchRoom", null);
            BehaviourTree.Blackboard.AddToBlackboard("SearchRoomCounter", 0);
            BehaviourTree.Blackboard.AddToBlackboard("ChaseTimer", 5.0f);

            // Creating actual behaviour tree
            Selector root = new Selector(BehaviourTree,
                new List<Node> {
                    // ! Stunned
                    new Sequence(BehaviourTree, new List<Node> {
                        new IsStunned(BehaviourTree),
                        new PlayAnimation(BehaviourTree, "stunned", true)
                    }),

                    // ! Player Dead
                    // new Sequence(BehaviourTree, new List<Node> {
                    //     new IsPlayerDead(BehaviourTree),
                    //     new IdleNode(BehaviourTree)
                    // }),

                    // ! Retreat
                    new Sequence(BehaviourTree, new List<Node> {
                        new Invertor(BehaviourTree, new IsBlackboardVariableNull(BehaviourTree, "retreat")),
                        new ShouldRetreat(BehaviourTree),

                        // Go to Retreat directly
                        new Selector(BehaviourTree, new List<Node> {
                            new Sequence(BehaviourTree, new List<Node> {
                                new IsInSameRoomAsVector3(BehaviourTree, "retreatPoint"),
                                new MoveToVector3(BehaviourTree, "retreatPoint")
                            }),

                            // Go to Retreat via Rooms
                            new Sequence(BehaviourTree, new List<Node> {
                                // Room Transition
                                new FindDoorPathToVector3(BehaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "retreatPoint"),
                                new GetDoorFromPath(BehaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor"),
                                new GetDoorEntryExitPoint(BehaviourTree, true, "TargetDoor", "MoveTarget"),
                                new Repeater(BehaviourTree, new MoveTo<Component>(BehaviourTree, "MoveTarget"), new IsAtTarget<Component>(BehaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),

                                // Room Transition
                                new Sequence(BehaviourTree, new List<Node> {
                                    new OpenDoor(BehaviourTree, "TargetDoor"),
                                    new SendAnimationRigSignal(BehaviourTree, "door", Animation.RigEvents.AnimRigEventType.ENABLE),
                                    new GetDoorEntryExitPoint(BehaviourTree, false, "TargetDoor", "MoveTarget"),
                                    new Repeater(BehaviourTree, new MoveTo<Component>(BehaviourTree, "MoveTarget"), new IsAtTarget<Component>(BehaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                                    new SendAnimationRigSignal(BehaviourTree, "door", Animation.RigEvents.AnimRigEventType.DISABLE),
                                    new IncrementDoorPathIndex(BehaviourTree, "TargetDoorPathIndex", "TargetDoorPath"),
                                }, true)
                            })
                        })
                    }),
                    
                    // ! NEW ATTACK
                    new ConditionalBranch(BehaviourTree, new CompareBlackboardVariable<MrXState>(BehaviourTree, MrXState.ATTACKING, "State"),
                        // Timer Selector
                        new Selector(BehaviourTree, new List<Node> {

                            // Reset Chase Timer if seen
                            new Sequence(BehaviourTree, new List<Node> {
                                new CanSeeObject(BehaviourTree),
                                new Invertor(BehaviourTree, new SetBlackboardVariable<float>(BehaviourTree, "ChaseTimer", 5.0f))
                            }),

                            // Usual Attacking Selector
                            new Selector(BehaviourTree, new List<Node> {
                                // * BECAUSE THIS IS ABOVE THE ATTACK MOVE LOGIC HE DOESN'T MOVE WHEN PLAYER IS IN VIEW WHEN ANIMATION IS PLAYING - maybe make selector between this and attacking selector?
                                // Decrement Chase Timer
                                new Sequence(BehaviourTree, new List<Node> {
                                    new AlterBlackboardVariableByDeltaTimeFloat(BehaviourTree, "ChaseTimer", AffectValueType.DECREMENT),
                                    new CompareBlackboardVariable<float>(BehaviourTree, 0.0f, "ChaseTimer", CompareType.LESS_THAN_EQUAL),
                                    new PlayAnimation(BehaviourTree, "looking_around", true),

                                    // Is Player in an inaccessible room?
                                    new Selector(BehaviourTree, new List<Node> {
                                        new Sequence(BehaviourTree, new List<Node> {
                                            new CanReachObjectRoom(BehaviourTree, "Player"),
                                            new GetRandomRoomAdjacentToTarget(BehaviourTree, true, "Player", "TargetSearchRoom")
                                        }),

                                        new GetRandomRoom(BehaviourTree, "TargetSearchRoom")
                                    }),
                                    
                                    new SetBlackboardVariable<MrXState>(BehaviourTree, "State", MrXState.SEARCHING)
                                }, false, true),
                                
                                // Atttacking Player
                                new Sequence(BehaviourTree, new List<Node> {
                                    new IsAtTarget<Component>(BehaviourTree, "Player", 1.0f),
                                    new PlayAnimation(BehaviourTree, "right_hook", true)
                                }),

                                // Move To Player
                                new Sequence(BehaviourTree, new List<Node> {
                                    new IsInSameRoomAs<Component>(BehaviourTree, "Player"),
                                    new MoveTo<Component>(BehaviourTree, "Player")
                                }),

                                // Room Navigation
                                new Sequence(BehaviourTree, new List<Node> {
                                    // Room Transition
                                    new FindDoorPathTo<Component>(BehaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "Player"),
                                    new GetDoorFromPath(BehaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor"),
                                    new GetDoorEntryExitPoint(BehaviourTree, true, "TargetDoor", "MoveTarget"),
                                    new Repeater(BehaviourTree, new MoveTo<Component>(BehaviourTree, "MoveTarget"), new IsAtTarget<Component>(BehaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),

                                    // Room Transition
                                    new Selector(BehaviourTree, new List<Node> {
                                        // Locked Door?
                                        new Sequence(BehaviourTree, new List<Node> {
                                            new Invertor(BehaviourTree, new CanUseDoor(BehaviourTree, "TargetDoor")),
                                            new CloseDoor(BehaviourTree, "TargetDoor"),
                                            new BangDoor(BehaviourTree, "TargetDoor"),
                                            new IdleNode(BehaviourTree, "IdleTimerLength"),
                                            new GetRandomRoom(BehaviourTree, "TargetSearchRoom"),
                                            new SetBlackboardVariable<MrXState>(BehaviourTree, "State", MrXState.SEARCHING)
                                        }),

                                        // Use Door
                                        new Sequence(BehaviourTree, new List<Node> {
                                            new OpenDoor(BehaviourTree, "TargetDoor"),
                                            new SendAnimationRigSignal(BehaviourTree, "door", Animation.RigEvents.AnimRigEventType.ENABLE),
                                            new GetDoorEntryExitPoint(BehaviourTree, false, "TargetDoor", "MoveTarget"),
                                            new Repeater(BehaviourTree, new MoveTo<Component>(BehaviourTree, "MoveTarget"), new IsAtTarget<Component>(BehaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                                            new SendAnimationRigSignal(BehaviourTree, "door", Animation.RigEvents.AnimRigEventType.DISABLE),
                                            new IncrementDoorPathIndex(BehaviourTree, "TargetDoorPathIndex", "TargetDoorPath"),
                                        }, true)
                                    })
                                })
                            })
                        })
                    ),

                    // ! Search
                    new ConditionalBranch(BehaviourTree, new CompareBlackboardVariable<MrXState>(BehaviourTree, MrXState.SEARCHING, "State"),
                        new Selector(BehaviourTree, new List<Node> {

                            // ! Init Search
                            new Sequence(BehaviourTree, new List<Node> {
                                new IsBlackboardVariableNull(BehaviourTree, "TargetSearchRoom"),
                                new GetRandomRoomAdjacentToTarget(BehaviourTree, true, "Player", "TargetSearchRoom")
                            }),

                            // ! Can See Player?
                            new Sequence(BehaviourTree, new List<Node> {
                                new CanSeeObject(BehaviourTree),
                                // new SetLastKnownLocation(BehaviourTree, "LastKnownLocation", "Player"),
                                new SetBlackboardVariable<MrXState>(BehaviourTree, "State", MrXState.ATTACKING)
                            }),

                            // ! Can Hear Player?
                            new Sequence(BehaviourTree, new List<Node> {
                                new HasHeardSound(BehaviourTree),
                                new CanReachObjectRoom(BehaviourTree, "Player"),
                                // new SetLastKnownLocation(BehaviourTree, "LastKnownLocation", "Player"),
                                new SetBlackboardVariable<MrXState>(BehaviourTree, "State", MrXState.ATTACKING)
                            }),

                            // ! Search Room + Go to Room
                            new Selector(BehaviourTree, new List<Node> {
                                // Search Room
                                new Sequence(BehaviourTree, new List<Node> {
                                    new IsInRoom(BehaviourTree, "TargetSearchRoom"),
                                    // Search Spots
                                    new Repeater(BehaviourTree,
                                        new Sequence(BehaviourTree, new List<Node> {
                                            new GetRandomRoomSearchSpot(BehaviourTree, "TargetSearchRoom", "MoveTarget"),
                                            new Repeater(BehaviourTree, new MoveTo<Component>(BehaviourTree, "MoveTarget"), new IsAtTarget<Component>(BehaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                                            new PlayAnimation(BehaviourTree, "looking_around", true),
                                            new IncrementBlackboardVariable(BehaviourTree, "SearchRoomCounter", 1)
                                        }),
                                        new CompareBlackboardVariable<int>(BehaviourTree, 1, "SearchRoomCounter"),
                                        NodeState.SUCCESSFUL
                                    ),
                                    new SetBlackboardVariable<int>(BehaviourTree, "SearchRoomCounter", 0),

                                    // Is Player in an inaccessible room?
                                    new Selector(BehaviourTree, new List<Node> {
                                        new Sequence(BehaviourTree, new List<Node> {
                                            new CanReachObjectRoom(BehaviourTree, "Player"),
                                            new GetRandomRoomAdjacentToTarget(BehaviourTree, true, "Player", "TargetSearchRoom")
                                        }),

                                        new GetRandomRoom(BehaviourTree, "TargetSearchRoom")
                                    }),
                                }),
                                
                                // Room Transition
                                new Sequence(BehaviourTree, new List<Node> {
                                    new FindDoorPathTo<Component>(BehaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetSearchRoom"),
                                    new GetDoorFromPath(BehaviourTree, "TargetDoorPath", "TargetDoorPathIndex", "TargetDoor"),
                                    new GetDoorEntryExitPoint(BehaviourTree, true, "TargetDoor", "MoveTarget"),
                                    new Repeater(BehaviourTree, new MoveTo<Component>(BehaviourTree, "MoveTarget"), new IsAtTarget<Component>(BehaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),

                                    new Selector(BehaviourTree, new List<Node> {
                                        // Locked Door?
                                        new Sequence(BehaviourTree, new List<Node> {
                                            new Invertor(BehaviourTree, new CanUseDoor(BehaviourTree, "TargetDoor")),
                                            new CloseDoor(BehaviourTree, "TargetDoor"),
                                            new BangDoor(BehaviourTree, "TargetDoor"),
                                            new IdleNode(BehaviourTree, "IdleTimerLength"),
                                            new GetRandomRoom(BehaviourTree, "TargetSearchRoom")
                                        }),

                                        // Use Door
                                        new Sequence(BehaviourTree, new List<Node> {
                                            new OpenDoor(BehaviourTree, "TargetDoor"),
                                            new SendAnimationRigSignal(BehaviourTree, "door", Animation.RigEvents.AnimRigEventType.ENABLE),
                                            new GetDoorEntryExitPoint(BehaviourTree, false, "TargetDoor", "MoveTarget"),
                                            new Repeater(BehaviourTree, new MoveTo<Component>(BehaviourTree, "MoveTarget"), new IsAtTarget<Component>(BehaviourTree, "MoveTarget", 0.2f), NodeState.SUCCESSFUL),
                                            new SendAnimationRigSignal(BehaviourTree, "door", Animation.RigEvents.AnimRigEventType.DISABLE),
                                            new IncrementDoorPathIndex(BehaviourTree, "TargetDoorPathIndex", "TargetDoorPath"),
                                        }, true)
                                    })


                                })
                            })
                        })
                    ),

                    // ! Idle
                    new IdleNode(BehaviourTree)
                }
            );

            return root;
        }

        private void Update()
        {
            BehaviourTree.EvaluateTree();
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

        public T PeekAtBlackBoardVariable<T>(string blackboardKey)
        {
            if (BehaviourTree == null) return default;

            return BehaviourTree.Blackboard.GetFromBlackboard<T>(blackboardKey);
        }
    }
}