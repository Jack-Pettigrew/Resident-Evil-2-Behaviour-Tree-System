using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorPathTo<T> : UpdateBlackboardService where T : Component
    {
        private readonly string doorPathBlackboardKey;
        private readonly string targetObjectBlackBoardKey;

        public GetDoorPathTo(BehaviourTree behaviourTree, string doorPathBlackboardKey, string targetObjectBlackBoardKey) : base(behaviourTree)
        {
            this.doorPathBlackboardKey = doorPathBlackboardKey;
            this.targetObjectBlackBoardKey = targetObjectBlackBoardKey;
        }

        protected override bool UpdateBlackboard()
        {
            return behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathBlackboardKey, RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject),
                RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<T>(targetObjectBlackBoardKey).gameObject)));

        }
    }
}