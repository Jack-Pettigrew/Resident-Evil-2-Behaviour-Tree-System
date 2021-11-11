using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Sequence : Node
    {
        private int currentNodeIndex = 0;
        protected List<Node> nodes;

        public Sequence(BehaviourTree behaviourTree, List<Node> nodes) : base(behaviourTree)
        {
            this.nodes = new List<Node>(nodes);
        }

        public override NodeState Evaluate()
        {
            // Process one at a time, stopping instantly at failure or haulting at running.
            switch (nodes[currentNodeIndex].Evaluate())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;

                case NodeState.SUCCESSFUL:
                    currentNodeIndex++;

                    if(currentNodeIndex >= nodes.Count) // Completed all Nodes
                    {
                        ResetSequence();
                        return NodeState.SUCCESSFUL;
                    }
                    else // More nodes to process
                    {
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