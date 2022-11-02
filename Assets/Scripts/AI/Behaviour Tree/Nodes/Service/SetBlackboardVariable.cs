using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class SetBlackboardVariable<T> : Service
    {
        protected readonly string blackboardKey;
        protected readonly T valueToSet;
        
        public SetBlackboardVariable(BehaviourTree behaviourTree, string blackboardKey, T valueToSet, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
            this.blackboardKey = blackboardKey;
            this.valueToSet = valueToSet;
        }

        protected override NodeState Evaluate()
        {
            return SetVariable() ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }

        protected virtual bool SetVariable()
        {
            return behaviourTree.Blackboard.UpdateBlackboardVariable(blackboardKey, valueToSet);
        }
    }
}