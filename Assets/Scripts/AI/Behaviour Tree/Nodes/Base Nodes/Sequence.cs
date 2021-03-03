using System.Collections.Generic;

namespace DD.AI.BehaviourTree
{
    public class Sequence : Node
    {
        protected List<Node> nodes = new List<Node>();

        public Sequence(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool isAnyNodeRunning = false;
            foreach (Node node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILED:
                        return NodeState.FAILED;
                    case NodeState.RUNNING:
                        isAnyNodeRunning = true;
                        break;

                    case NodeState.SUCCESSFUL:
                    default:
                        break;
                }
            }

            return isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESSFUL;
        }
    }
}