using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class CanReachObjectRoom : Conditional
    {
        protected readonly string objectBlackboardKey;
        
        public CanReachObjectRoom(BehaviourTree behaviourTree, string objectBlackboardKey) : base(behaviourTree)
        {
            this.objectBlackboardKey = objectBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Room ourRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject);
            Room objectRoom = RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<Component>(objectBlackboardKey).gameObject);

            return RoomPathFinder.DoesPathToRoomExist(ourRoom, objectRoom) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
