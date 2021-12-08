using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class HasPathToRoom : Conditional
    {
        private readonly string doorPathBlackboardKey;
        private readonly string targetRoomBlackboardKey;

        public HasPathToRoom(BehaviourTree behaviourTree, string doorPathBlackboardKey, string targetRoomBlackboardKey) : base(behaviourTree)
        {
            this.doorPathBlackboardKey = doorPathBlackboardKey;
            this.targetRoomBlackboardKey = targetRoomBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Door[] path = behaviourTree.Blackboard.GetFromBlackboard<Door[]>(doorPathBlackboardKey);
            Room targetRoom = behaviourTree.Blackboard.GetFromBlackboard<Room>(targetRoomBlackboardKey);

            if (path != null)
            {
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
