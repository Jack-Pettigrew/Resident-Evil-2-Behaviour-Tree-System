using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoom : UpdateBlackboardService
    {
        public GetRandomRoom(BehaviourTree behaviourTree, string targetDoorBlackboardVariable) : base(behaviourTree, targetDoorBlackboardVariable)
        {
        }

        protected override NodeState Evaluate()
        {
            Room result;
            Room currentRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject);

            // Randomise until Room isn't current one
            do
            {
                result = RoomManager.GetRandomRoom();
            } while (result == currentRoom);

            return UpdateBlackboard(result) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}