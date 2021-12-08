using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorPathToRoom : UpdateBlackboardService
    {
        private readonly string targetRoomBlackboardKey;
        private readonly string doorPathIndexBlackboardKey;

        public GetDoorPathToRoom(BehaviourTree behaviourTree, string roomPathArrayBlackboardKey, string targetBlackboardKey, string doorPathIndexBlackboardKey) : base(behaviourTree, roomPathArrayBlackboardKey)
        {
            this.targetRoomBlackboardKey = targetBlackboardKey;
            this.doorPathIndexBlackboardKey = doorPathIndexBlackboardKey;
        }

        protected override bool UpdateBlackboard(object updatedBlackboardVariable)
        {
            behaviourTree.Blackboard.UpdateBlackboardVariable(doorPathIndexBlackboardKey, 0);
            return behaviourTree.Blackboard.UpdateBlackboardVariable(variableBlackboardKey, updatedBlackboardVariable);
        }

        protected override NodeState Evaluate()
        {
            return UpdateBlackboard(RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject), 
                behaviourTree.Blackboard.GetFromBlackboard<Room>(targetRoomBlackboardKey))) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}