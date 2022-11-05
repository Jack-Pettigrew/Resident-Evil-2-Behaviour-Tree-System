using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoomAdjacentToTarget : Service
    {
        private readonly bool includeTargetObjectRoom;
        private readonly string targetObjectBlackboardKey;
        private readonly string resultingRoomBlackboardKey;
        
        public GetRandomRoomAdjacentToTarget(BehaviourTree behaviourTree, bool includeTargetObjectRoom, string targetObjectBlackboardKey, string resultingRoomBlackboardKey) : base(behaviourTree)
        {
            this.includeTargetObjectRoom = includeTargetObjectRoom;
            this.targetObjectBlackboardKey = targetObjectBlackboardKey;
            this.resultingRoomBlackboardKey = resultingRoomBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            Room targetsRoom = RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<Component>(targetObjectBlackboardKey).gameObject);

            Room searchRoom = RoomManager.GetRandomAdjacentRoom(targetsRoom, includeTargetObjectRoom);
            
            if(searchRoom == null) 
            {
                return NodeState.FAILED;
            }

            return behaviourTree.Blackboard.UpdateBlackboardVariable(resultingRoomBlackboardKey, searchRoom) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
