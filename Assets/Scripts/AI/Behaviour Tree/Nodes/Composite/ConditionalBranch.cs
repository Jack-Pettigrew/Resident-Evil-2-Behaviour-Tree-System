using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// A conditional branching node that runs it's child given the conditional node is successful.
    /// </summary>
    public class ConditionalBranch : Composite
    {
        protected Node branchingNode;
        protected Conditional conditionalNode;
        
        public ConditionalBranch(BehaviourTree behaviourTree, Node branchingNode, Conditional conditionalNode, bool uninterruptable) : base(behaviourTree, null, uninterruptable)
        {
            this.branchingNode = branchingNode;
            this.conditionalNode = conditionalNode;
        }

        protected override NodeState Evaluate()
        {
            if(conditionalNode.UpdateNode() == NodeState.SUCCESSFUL)
            {
                return UpdateChildNode(branchingNode);
            }

            return NodeState.FAILED;
        }
    }
}