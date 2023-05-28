using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class FindDoorPathToVector3 : UpdateBlackboardService
    {
        protected readonly string doorPathBlackboardKey;
        protected readonly string doorPathIndexBlackboardKey;
        protected readonly string targetVector3BlackBoardKey;

        public FindDoorPathToVector3(BehaviourTree behaviourTree, string doorPathBlackboardKey, string doorPathIndexBlackboardKey, string targetVector3BlackBoardKey) : base(behaviourTree)
        {
            this.doorPathBlackboardKey = doorPathBlackboardKey;
            this.doorPathIndexBlackboardKey = doorPathIndexBlackboardKey;
            this.targetVector3BlackBoardKey = targetVector3BlackBoardKey;
        }

        protected override bool UpdateBlackboard()
        {
            ResetDoorPathIndex();

            Room startRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject);
            Room goalRoom = RoomManager.GetRoomOfPoint(behaviourTree.Blackboard.GetFromBlackboard<Vector3>(targetVector3BlackBoardKey));

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
