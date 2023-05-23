using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoom : UpdateBlackboardService
    {
        public readonly string targetRoomBlackboardVariable;

        public GetRandomRoom(BehaviourTree behaviourTree, string targetRoomBlackboardVariable) : base(behaviourTree)
        {
            this.targetRoomBlackboardVariable = targetRoomBlackboardVariable;
        }

        protected override bool UpdateBlackboard()
        {
            Room result;
            Room currentRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject);

            if(!currentRoom)
            {
                return false;
            }

            // Randomise until Room isn't current one
            do
            {
                result = RoomManager.GetRandomRoom();
            } while (result == currentRoom);

            return behaviourTree.Blackboard.UpdateBlackboardVariable(targetRoomBlackboardVariable, result);
        }
    }
}