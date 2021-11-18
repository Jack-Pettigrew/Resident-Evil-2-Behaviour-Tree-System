using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsDoorOpenNode : Conditional
    {
        public readonly string targetDoorBlackboardKey;

        public IsDoorOpenNode(BehaviourTree behaviourTree, string targetDoorBlackboardKey) : base(behaviourTree)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).IsOpen ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }

}