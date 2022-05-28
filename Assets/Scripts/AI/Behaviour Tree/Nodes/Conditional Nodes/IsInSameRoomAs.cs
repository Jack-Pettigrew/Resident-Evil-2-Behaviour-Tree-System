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
            return RoomManager.IsObjectInRoom(
                behaviourTree.ai.GetAITransform().gameObject,
                RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<T>(targetBlackboardKey).gameObject)
            ) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}