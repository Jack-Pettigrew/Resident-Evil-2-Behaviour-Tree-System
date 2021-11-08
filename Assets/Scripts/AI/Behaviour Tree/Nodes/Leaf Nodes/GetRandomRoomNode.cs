using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoomNode : Node
    {
        private readonly bool unsearchedRoomsOnly;
        private readonly IAIBehaviour ai;
        private readonly string roomBlackboardKey;

        public GetRandomRoomNode(IAIBehaviour ai, string roomBlackboardKey, bool unsearchedRoomsOnly = false)
        {
            this.ai = ai;
            this.roomBlackboardKey = roomBlackboardKey;
            this.unsearchedRoomsOnly = unsearchedRoomsOnly;
        }

        public override NodeState Evaluate()
        {
            Room result;
            Room currentRoom = RoomManager.GetRoomOfObject(ai.GetAITransform().position);

            do
            {
                result = RoomManager.GetRandomRoom();
            } while (result == currentRoom);

            ai.GetAIBlackboard().UpdateBlackboardVariable(roomBlackboardKey, result, true);

            return NodeState.SUCCESSFUL;
        }
    }
}