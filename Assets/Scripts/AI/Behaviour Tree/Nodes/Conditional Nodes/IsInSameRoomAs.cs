using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsInSameRoomAs<T> : Conditional where T : Component
    {
        private readonly string targetBlackboardKey;

        public IsInSameRoomAs(BehaviourTree behaviourTree, string targetBlackboardKey) : base(behaviourTree)
        {
            this.targetBlackboardKey = targetBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Room targetRoom = RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<T>(targetBlackboardKey).gameObject);

            if(!targetRoom)
            {
                return NodeState.FAILED;
            }
            
            return RoomManager.IsObjectInRoom(
                behaviourTree.ai.GetAITransform().gameObject,
                targetRoom
            ) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}