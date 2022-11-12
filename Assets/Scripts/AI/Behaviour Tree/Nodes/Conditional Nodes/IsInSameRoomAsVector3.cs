using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsInSameRoomAsVector3 : Conditional
    {
        private readonly string targetVectorBlackboardKey;
        
        public IsInSameRoomAsVector3(BehaviourTree behaviourTree, string targetVectorBlackboardKey) : base(behaviourTree)
        {
            this.targetVectorBlackboardKey = targetVectorBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Room targetRoom = RoomManager.GetRoomOfPoint(behaviourTree.Blackboard.GetFromBlackboard<Vector3>(targetVectorBlackboardKey));

            if(!targetRoom)
            {
                return NodeState.FAILED;
            }
            
            return RoomManager.IsObjectInRoom(
                behaviourTree.ai.GetAITransform().gameObject,
                targetRoom
            ) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
