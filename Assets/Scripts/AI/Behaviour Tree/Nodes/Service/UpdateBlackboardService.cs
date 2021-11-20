using System;
using System.Collections;
using System.Collections.Generic;

namespace DD.AI.BehaviourTreeSystem
{
    public abstract class UpdateBlackboardService : Service
    {
        protected readonly string variableBlackboardKey;

        public UpdateBlackboardService(BehaviourTree behaviourTree, string variableBlackboardKey) : base(behaviourTree)
        {
            this.variableBlackboardKey = variableBlackboardKey;
        }

        protected virtual bool UpdateBlackboard(object updatedBlackboardVariable)
        {
            return behaviourTree.Blackboard.UpdateBlackboardVariable(variableBlackboardKey, updatedBlackboardVariable, true);
        }
    }
}