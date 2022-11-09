using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// Gets a random search spot from a given Room and set it as a blackboard variable. Gets Room label location if no search spots can be found.
    /// </summary>
    public class GetRandomRoomSearchSpot : Service
    {
        private readonly string roomBlackboardKey;
        private readonly string moveTargetBlackboardKey;

        public GetRandomRoomSearchSpot(BehaviourTree behaviourTree, string roomBlackboardKey, string moveTargetBlackboardKey) : base(behaviourTree)
        {
            this.roomBlackboardKey = roomBlackboardKey;
            this.moveTargetBlackboardKey = moveTargetBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            Room room = behaviourTree.Blackboard.GetFromBlackboard<Room>(roomBlackboardKey);

            if(room.SearchSpots.Length == 0)
            {
                behaviourTree.Blackboard.UpdateBlackboardVariable(moveTargetBlackboardKey, room.transform);
            }
            else
            {
                behaviourTree.Blackboard.UpdateBlackboardVariable(moveTargetBlackboardKey, room.SearchSpots[Random.Range(0, room.SearchSpots.Length)]);
            }

            return NodeState.SUCCESSFUL;
        }
    }
}
