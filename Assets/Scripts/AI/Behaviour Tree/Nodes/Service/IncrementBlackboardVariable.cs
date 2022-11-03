using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class IncrementBlackboardVariable : Service
    {
        private readonly string variableBlackboardKey;
        private readonly int incrementAmount;
        
        public IncrementBlackboardVariable(BehaviourTree behaviourTree, string variableBlackboardKey, int incrementAmount) : base(behaviourTree)
        {
            this.variableBlackboardKey = variableBlackboardKey;
            this.incrementAmount = incrementAmount;
        }

        protected override NodeState Evaluate()
        {
            return behaviourTree.Blackboard.UpdateBlackboardVariable(variableBlackboardKey, behaviourTree.Blackboard.GetFromBlackboard<int>(variableBlackboardKey) + incrementAmount) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
