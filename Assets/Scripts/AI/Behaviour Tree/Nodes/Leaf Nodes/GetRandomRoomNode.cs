using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoomNode : Node
    {
        private readonly string roomBlackboardKey;

        public GetRandomRoomNode(BehaviourTree behaviourTree, string roomBlackboardKey) : base(behaviourTree)
        {
            this.roomBlackboardKey = roomBlackboardKey;
        }

        public override NodeState Evaluate()
        {
            Room result;
            Room currentRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().position);

            do
            {
                result = RoomManager.GetRandomRoom();
            } while (result == currentRoom);

            behaviourTree.Blackboard.UpdateBlackboardVariable(roomBlackboardKey, result, true);

            return NodeState.SUCCESSFUL;
        }
    }
}