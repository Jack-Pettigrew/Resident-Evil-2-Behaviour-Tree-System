using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class FindPathToGameObject : UpdateBlackboardService
    {
        private readonly string targetObjectBlackBoardKey;

        public FindPathToGameObject(BehaviourTree behaviourTree, string doorPathBlackboardKey, string targetObjectBlackBoardKey) : base(behaviourTree, doorPathBlackboardKey)
        {
            this.targetObjectBlackBoardKey = targetObjectBlackBoardKey;
        }

        protected override NodeState Evaluate()
        {
            return UpdateBlackboard(RoomPathFinder.FindPathToRoom(RoomManager.GetRoomOfObject(behaviourTree.ai.GetAITransform().gameObject),
                RoomManager.GetRoomOfObject(behaviourTree.Blackboard.GetFromBlackboard<GameObject>(targetObjectBlackBoardKey)))) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}