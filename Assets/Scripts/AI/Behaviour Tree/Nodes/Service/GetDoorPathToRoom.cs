using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorPathToRoom : GetDoorPathTo<Room>
    {
        public GetDoorPathToRoom(BehaviourTree behaviourTree, string doorPathBlackboardKey, string doorPathIndexBlackboardKey, string roomTargetBlackboardKey) : base(behaviourTree, doorPathBlackboardKey, doorPathIndexBlackboardKey, roomTargetBlackboardKey)
        {
        }

        protected override bool UpdateBlackboard()
        {
            ResetDoorPathIndex();

            return behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathBlackboardKey, 
                RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject), behaviourTree.Blackboard.GetFromBlackboard<Room>(targetObjectBlackBoardKey))
            );
        }
    }
}