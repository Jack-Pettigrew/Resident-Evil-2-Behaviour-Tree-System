using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GoToDoor : MoveTo<Door>
    {
        public GoToDoor(BehaviourTree behaviourTree, string targetBlackboardKey) : base(behaviourTree, targetBlackboardKey)
        {
        }

        protected override Vector3 GetTargetPosition()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<Door>(targetBlackboardKey)
                .GetDoorEntryPointRelativeToGivenRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject)).transform.position;
        }
    }
}