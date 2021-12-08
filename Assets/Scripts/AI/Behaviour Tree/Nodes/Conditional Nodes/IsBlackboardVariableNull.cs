using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsBlackboardVariableNull : Conditional
    {
        private readonly string blackboardVariableKey;

        public IsBlackboardVariableNull(BehaviourTree behaviourTree, string blackboardVariableKey) : base(behaviourTree)
        {
            this.blackboardVariableKey = blackboardVariableKey;
        }

        protected override NodeState EvaluateConditional()
        {
            return behaviourTree.Blackboard.IsBlackboardVariableNull(blackboardVariableKey) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}