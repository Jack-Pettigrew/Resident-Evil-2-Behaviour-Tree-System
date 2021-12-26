using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorPathTo<T> : UpdateBlackboardService where T : Component
    {
        protected readonly string doorPathBlackboardKey;
        protected readonly string doorPathIndexBlackboardKey;
        protected readonly string targetObjectBlackBoardKey;

        public GetDoorPathTo(BehaviourTree behaviourTree, string doorPathBlackboardKey, string doorPathIndexBlackboardKey, string targetObjectBlackBoardKey) : base(behaviourTree)
        {
            this.doorPathBlackboardKey = doorPathBlackboardKey;
            this.doorPathIndexBlackboardKey = doorPathIndexBlackboardKey;
            this.targetObjectBlackBoardKey = targetObjectBlackBoardKey;
        }

        protected override bool UpdateBlackboard()
        {
            ResetDoorPathIndex();

            return behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathBlackboardKey, 
                RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject), RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<T>(targetObjectBlackBoardKey).gameObject))
            );

        }

        protected void ResetDoorPathIndex()
        {
            behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathIndexBlackboardKey, 0);
        }
    }
}