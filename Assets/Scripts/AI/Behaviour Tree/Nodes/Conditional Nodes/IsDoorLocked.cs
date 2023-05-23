using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsDoorLocked : Conditional
    {
        private readonly string doorBlackboardKey;

        public IsDoorLocked(BehaviourTree behaviourTree, string doorBlackboardKey) : base(behaviourTree)
        {
            this.doorBlackboardKey = doorBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<Door>(doorBlackboardKey).IsLocked ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}