using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Sequence : Composite
    {
        private int currentNodeIndex = 0;

        public Sequence(BehaviourTree behaviourTree, List<Node> childNodes, bool uninterruptable = false) : base(behaviourTree, childNodes, uninterruptable)
        { }

        protected override NodeState Evaluate()
        {            
            // Uninterruptable Check
            if(childNodes[currentNodeIndex].IsUninterruptable && childNodes[currentNodeIndex].State != NodeState.NONE)
            {
                return childNodes[currentNodeIndex].State;
            }
            
            // Evaluate one node at a time, haulting at running or stopping at failure
            switch (childNodes[currentNodeIndex].UpdateNode())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;

                case NodeState.SUCCESSFUL:
                    currentNodeIndex++;

                    // IF finished all child nodes...
                    if(currentNodeIndex >= childNodes.Count)
                    {
                        currentNodeIndex = 0;
                        return NodeState.SUCCESSFUL;
                    }
                    else
                    {
                        // Continue to evaluate other nodes
                        return Evaluate();
                    }

                case NodeState.FAILED:
                default:
                    currentNodeIndex = 0;
                    return NodeState.FAILED;    // Node failed, we should fail
            }
        }

        protected override void OnReset()
        {
            base.OnReset();

            currentNodeIndex = 0;
        }
    }
}