using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class FindDoorPathTo<T> : UpdateBlackboardService where T : Component
    {
        protected readonly string doorPathBlackboardKey;
        protected readonly string doorPathIndexBlackboardKey;
        protected readonly string targetObjectBlackBoardKey;

        public FindDoorPathTo(BehaviourTree behaviourTree, string doorPathBlackboardKey, string doorPathIndexBlackboardKey, string targetObjectBlackBoardKey) : base(behaviourTree)
        {
            this.doorPathBlackboardKey = doorPathBlackboardKey;
            this.doorPathIndexBlackboardKey = doorPathIndexBlackboardKey;
            this.targetObjectBlackBoardKey = targetObjectBlackBoardKey;
        }

        protected override bool UpdateBlackboard()
        {
            ResetDoorPathIndex();

            Room startRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject);
            Room goalRoom = RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<T>(targetObjectBlackBoardKey).gameObject);

            if(!startRoom || !goalRoom)
            {
                return false;
            }

            return behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathBlackboardKey, RoomPathFinder.FindDoorPathToRoom(startRoom, goalRoom, false));

        }

        protected void ResetDoorPathIndex()
        {
            behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathIndexBlackboardKey, 0);
        }
    }
}