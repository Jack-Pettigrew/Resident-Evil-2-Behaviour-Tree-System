using System;
using System.Collections;
using System.Collections.Generic;

namespace DD.AI.BehaviourTreeSystem
{
    public abstract class UpdateBlackboardService : Service
    {
        public UpdateBlackboardService(BehaviourTree behaviourTree) : base(behaviourTree)
        {
        }

        protected override NodeState Evaluate()
        {
            return UpdateBlackboard() ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }

        protected abstract bool UpdateBlackboard();
    }
}