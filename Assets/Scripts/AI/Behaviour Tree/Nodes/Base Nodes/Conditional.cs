using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public abstract class Conditional : Node
    {
        protected Node trueNode = null, falseNode = null;

        protected abstract Node EvaluateConditional();

        public override NodeState Evaluate()
        {
            Node resultNode = EvaluateConditional();

            if(resultNode == null)
            {
                return NodeState.FAILED;
            }

            switch (resultNode.Evaluate())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.SUCCESSFUL:
                    return NodeState.SUCCESSFUL;

                case NodeState.FAILED:
                default:
                    break;
            }

            return NodeState.FAILED;
        }
    }
}