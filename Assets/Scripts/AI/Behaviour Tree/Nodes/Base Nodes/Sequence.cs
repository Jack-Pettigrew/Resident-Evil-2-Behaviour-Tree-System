using System.Collections.Generic;

namespace DD.AI.BehaviourTreeSystem
{
    public class Sequence : Node
    {
        protected List<Node> nodes = new List<Node>();
        private int currentNodeIndex = 0;

        public Sequence(BehaviourTree tree, List<Node> nodes) : base(tree)
        {
            this.nodes = nodes;
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
                        return NodeState.RUNNING;
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