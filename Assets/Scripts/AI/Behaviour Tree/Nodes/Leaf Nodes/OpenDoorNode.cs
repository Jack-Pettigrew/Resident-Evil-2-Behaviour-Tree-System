using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class OpenDoorNode : Node
    {
        private readonly string targetDoorBlackboardKey;

        public OpenDoorNode(BehaviourTree behaviourTree, string targetDoorBlackboardKey) : base(behaviourTree)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).OpenDoor() ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}