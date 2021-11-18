using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class CloseDoorNode : Node
    {
        private readonly string targetDoorBlackboardKey;

        public CloseDoorNode(BehaviourTree behaviourTree, string targetDoorBlackboardKey) : base(behaviourTree)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).CloseDoor() ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
