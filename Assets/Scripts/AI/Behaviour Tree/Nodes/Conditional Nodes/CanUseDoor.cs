using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class CanUseDoor : Conditional
    {
        protected readonly string targetDoorBlackboardKey;
        
        public CanUseDoor(BehaviourTree behaviourTree, string targetDoorBlackboardKey) : base(behaviourTree)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
        }
        
        protected override NodeState EvaluateConditional()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).CanAIUse ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}