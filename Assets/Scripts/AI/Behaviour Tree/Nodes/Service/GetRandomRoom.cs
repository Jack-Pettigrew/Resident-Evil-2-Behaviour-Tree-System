using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetRandomRoom : UpdateBlackboardService
    {
        public readonly string targetDoorBlackboardVariable;

        public GetRandomRoom(BehaviourTree behaviourTree, string targetDoorBlackboardVariable) : base(behaviourTree)
        {
            this.targetDoorBlackboardVariable = targetDoorBlackboardVariable;
        }

        protected override bool UpdateBlackboard()
        {
            Room result;
            Room currentRoom = RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject);

            // Randomise until Room isn't current one
            do
            {
                result = RoomManager.GetRandomRoom();
            } while (result == currentRoom);

            return behaviourTree.Blackboard.UpdateBlackboardVariable(targetDoorBlackboardVariable, result);
        }
    }
}