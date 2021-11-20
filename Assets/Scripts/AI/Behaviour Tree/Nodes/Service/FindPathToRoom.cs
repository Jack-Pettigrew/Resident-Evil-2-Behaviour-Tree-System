using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class FindPathToRoom : UpdateBlackboardService
    {
        private readonly string targetRoomBlackboardKey;

        public FindPathToRoom(BehaviourTree behaviourTree, string roomPathArrayBlackboardKey, string targetBlackboardKey) : base(behaviourTree, roomPathArrayBlackboardKey)
        {
            this.targetRoomBlackboardKey = targetBlackboardKey;
        }

        protected override NodeState Evaluate()
        { 
            return UpdateBlackboard(RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject), 
                behaviourTree.Blackboard.GetFromBlackboard<Room>(targetRoomBlackboardKey))) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}