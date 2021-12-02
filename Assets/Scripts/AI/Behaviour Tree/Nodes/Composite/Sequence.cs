using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Sequence : Composite
    {
        private int currentNodeIndex = 0;

        public Sequence(BehaviourTree behaviourTree, List<Node> childNodes) : base(behaviourTree, childNodes)
        {
        }

        protected override NodeState Evaluate()
        {
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
                        ResetSequence();
                        return NodeState.SUCCESSFUL;
                    }
                    else
                    {
                        // Continue to evaluate other nodes
                        return Evaluate();
                    }

                case NodeState.FAILED:
                default:
                    ResetSequence();
                    return NodeState.FAILED;    // Node failed, we should fail
            }
        }

        private void ResetSequence()
        {
            currentNodeIndex = 0;
        }
    }
}