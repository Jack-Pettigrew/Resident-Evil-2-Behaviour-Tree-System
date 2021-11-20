using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoomNode : UpdateBlackboardService
    {
        public GetRandomRoomNode(BehaviourTree behaviourTree, string blackboardKey) : base(behaviourTree, blackboardKey)
        {
        }

        protected override NodeState Evaluate()
        {
            Room result;
            Room currentRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().position);

            // Randomise until Room isn't current one
            do
            {
                result = RoomManager.GetRandomRoom();
            } while (result == currentRoom);

            return UpdateBlackboard(result) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}