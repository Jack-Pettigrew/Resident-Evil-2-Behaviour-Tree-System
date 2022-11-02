using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class CompareBlackboardVariable<T> : Conditional
    {
        private readonly T comparerVariable;
        private readonly string blackboardVariableKey;

        public CompareBlackboardVariable(BehaviourTree behaviourTree, T comparerVariable, string blackboardVariableKey, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
            this.comparerVariable = comparerVariable;
            this.blackboardVariableKey = blackboardVariableKey;
        }

        protected override NodeState EvaluateConditional()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<T>(blackboardVariableKey).Equals(comparerVariable) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
