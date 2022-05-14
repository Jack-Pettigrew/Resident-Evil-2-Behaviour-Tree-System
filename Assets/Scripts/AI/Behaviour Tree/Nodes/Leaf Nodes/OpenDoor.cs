using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class OpenDoor : LeafNode
    {
        private readonly string targetDoorBlackboardKey;

        public OpenDoor(BehaviourTree behaviourTree, string targetDoorBlackboardKey) : base(behaviourTree)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).OpenDoor() ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}