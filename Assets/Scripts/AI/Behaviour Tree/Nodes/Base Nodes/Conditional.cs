using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// Conditional Node - can be pure conidtional (no nodes, only returning Success or Fail states) or can be branching conditional (has nodes, acts branching Composite Node).
    /// </summary>
    public abstract class Conditional : Node
    {
        protected bool pureConditional = false;
        protected Node trueNode = null, falseNode = null;

        protected Controllers.IAIBehaviour ai;

        /// <summary>
        /// Pure Conditional Constructor.
        /// </summary>
        protected Conditional(Controllers.IAIBehaviour ai)
        {
            pureConditional = true;
            this.ai = ai;
        }

        /// <summary>
        /// Branching Conditional Constructor.
        /// </summary>
        protected Conditional(Node trueNode, Node falseNode)
        {
            this.trueNode = trueNode;
            this.falseNode = falseNode;
        }

        protected abstract NodeState EvaluateConditional();

        public override NodeState Evaluate()
        {
            return pureConditional ? EvaluateConditional() : EvaluateBranchingConditional();
        }

        private NodeState EvaluateBranchingConditional()
        {
            if (trueNode == null || falseNode == null)
            {
                throw new System.Exception($"Conditional Node: {this} has null truth and/or false Nodes");
            }

            Node resultNode = EvaluateConditional() == NodeState.SUCCESSFUL ? trueNode : falseNode;

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