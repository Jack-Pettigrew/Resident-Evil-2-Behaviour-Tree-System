using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class FindPathToRoom : UpdateBlackboardService
    {
        private readonly string targetRoomBlackboardKey;

        public FindPathToRoom(BehaviourTree behaviourTree, string roomPathArrayBlackboardKey, string targetRoomBlackboardKey) : base(behaviourTree, roomPathArrayBlackboardKey)
        {
            this.targetRoomBlackboardKey = targetRoomBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            return UpdateBlackboard(RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().position), 
                behaviourTree.Blackboard.GetFromBlackboard<Room>(targetRoomBlackboardKey))) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}