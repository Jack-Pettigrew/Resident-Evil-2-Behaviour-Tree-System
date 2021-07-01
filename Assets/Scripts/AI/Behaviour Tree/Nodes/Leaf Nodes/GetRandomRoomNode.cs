using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoomNode : Node
    {
        private readonly int retryLimit;
        private readonly bool unsearchedRoomsOnly;
        private readonly IAIBehaviour ai;
        private readonly string roomBlackboardKey;

        public GetRandomRoomNode(IAIBehaviour ai, string roomBlackboardKey, bool unsearchedRoomsOnly = false, int retryLimit = 0)
        {
            this.ai = ai;
            this.roomBlackboardKey = roomBlackboardKey;
            this.unsearchedRoomsOnly = unsearchedRoomsOnly;
            this.retryLimit = retryLimit;
        }

        public override NodeState Evaluate()
        {
            int tries = 0;
            Room result;
            Room currentRoom = ai.GetAIBlackboard().GetFromBlackboard<Room>(roomBlackboardKey);

            do
            {
                result = RoomManager.GetRandomRoom();
                ++tries;

                if(tries <= retryLimit)
                {
                    break;
                }

            } while (result == currentRoom);

            ai.GetAIBlackboard().UpdateBlackboardVariable(roomBlackboardKey, result, true);

            return NodeState.SUCCESSFUL;
        }
    }
}