using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class HasRoomPathTo<T> : Conditional where T : Component
    {
        protected readonly string doorPathBlackboardKey;
        protected readonly string targetBlackboardKey;

        public HasRoomPathTo(BehaviourTree behaviourTree, string doorPathBlackboardKey, string targetBlackboardKey) : base(behaviourTree)
        {
            this.doorPathBlackboardKey = doorPathBlackboardKey;
            this.targetBlackboardKey = targetBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Door[] path = behaviourTree.Blackboard.GetFromBlackboard<Door[]>(doorPathBlackboardKey);

            if (path != null)
            {
                Room targetRoom = RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<T>(targetBlackboardKey).gameObject);

                if (path[path.Length - 1].RoomA == targetRoom || path[path.Length - 1].RoomB == targetRoom)
                {
                    return NodeState.SUCCESSFUL;
                }
                else
                {
                    return NodeState.FAILED;
                }
            }

            return NodeState.FAILED;
        }

    }
}