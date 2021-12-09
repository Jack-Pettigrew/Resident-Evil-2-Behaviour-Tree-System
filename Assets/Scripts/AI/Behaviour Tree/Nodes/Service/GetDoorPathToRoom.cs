using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorPathToRoom : UpdateBlackboardService
    {
        private readonly string roomPathArrayBlackboardKey;
        private readonly string targetRoomBlackboardKey;
        private readonly string doorPathIndexBlackboardKey;

        public GetDoorPathToRoom(BehaviourTree behaviourTree, string roomPathArrayBlackboardKey, string targetBlackboardKey, string doorPathIndexBlackboardKey) : base(behaviourTree)
        {
            this.roomPathArrayBlackboardKey = roomPathArrayBlackboardKey;
            this.targetRoomBlackboardKey = targetBlackboardKey;
            this.doorPathIndexBlackboardKey = doorPathIndexBlackboardKey;
        }

        protected override bool UpdateBlackboard()
        {
            behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathIndexBlackboardKey, 0);

            return behaviourTree.Blackboard.UpdateBlackboardVariable(roomPathArrayBlackboardKey, 
                RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject), behaviourTree.Blackboard.GetFromBlackboard<Room>(targetRoomBlackboardKey))
            );
        }

        protected override NodeState Evaluate()
        {
            return UpdateBlackboard() ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}