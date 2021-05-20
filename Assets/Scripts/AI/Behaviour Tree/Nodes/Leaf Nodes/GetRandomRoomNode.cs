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
        private readonly string blackboardKey;

        public GetRandomRoomNode(bool unsearchedRoomsOnly, int retryLimit, IAIBehaviour ai, string blackboardKey)
        {
            this.unsearchedRoomsOnly = unsearchedRoomsOnly;
            this.ai = ai;
            this.blackboardKey = blackboardKey;

            ai.GetAIBlackboard().AddToBlackboard(blackboardKey, new List<int>());
        }

        public override NodeState Evaluate()
        {
            int tries = 0;
            Room result;
            Room currentRoom = ai.GetAIBlackboard().GetFromBlackboard<Room>(blackboardKey);

            do
            {
                result = RoomManager.GetRandomRoom();
                ++tries;

                if(tries < retryLimit)
                {
                    break;
                }

            } while (result == currentRoom);

            return NodeState.SUCCESSFUL;
        }
    }
}