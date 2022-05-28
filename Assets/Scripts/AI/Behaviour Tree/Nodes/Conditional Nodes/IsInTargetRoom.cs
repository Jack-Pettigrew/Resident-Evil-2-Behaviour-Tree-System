using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsInTargetRoom : Conditional
    {
        private readonly string targetRoomBlackboardKey;

        public IsInTargetRoom(BehaviourTree behaviourTree, string targetRoomBlackboardKey) : base(behaviourTree)
        {
            this.targetRoomBlackboardKey = targetRoomBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            return RoomManager.IsObjectInRoom(behaviourTree.ai.GetAITransform().gameObject, behaviourTree.Blackboard.GetFromBlackboard<Room>(targetRoomBlackboardKey)) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
