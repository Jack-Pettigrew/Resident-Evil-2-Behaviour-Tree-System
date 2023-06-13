using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class ShouldRetreat : Conditional
    {
        public ShouldRetreat(BehaviourTree behaviourTree) : base(behaviourTree)
        {
        }

        protected override NodeState EvaluateConditional()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<bool>("retreat") ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
