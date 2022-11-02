using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// Conditional Node - returns Success or Fail states based on condtion logic.
    /// </summary>
    public abstract class Conditional : Node
    {
        protected Conditional(BehaviourTree behaviourTree) : base(behaviourTree, false)
        {
        }

        /// <summary>
        /// Performs the evaluation logic of this conditional node.
        /// </summary>
        /// <returns>Node State</returns>
        protected abstract NodeState EvaluateConditional();

        protected override NodeState Evaluate()
        {
            return EvaluateConditional();
        }
    }
}