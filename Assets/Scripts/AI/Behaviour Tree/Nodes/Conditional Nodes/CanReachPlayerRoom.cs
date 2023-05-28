using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class CanReachPlayerRoom : Conditional
    {
        protected readonly string playerBlackboardKey;
        
        public CanReachPlayerRoom(BehaviourTree behaviourTree, string playerBlackboardKey) : base(behaviourTree)
        {
            this.playerBlackboardKey = playerBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Room ourRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject);
            Room playerRoom = RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<Component>(playerBlackboardKey).gameObject);

            return RoomPathFinder.DoesPathToRoomExist(ourRoom, playerRoom) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
