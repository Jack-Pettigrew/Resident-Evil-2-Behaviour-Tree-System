using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Selector : Node
    {
        protected List<Node> nodes = new List<Node>();

        public Selector(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            // Process all until Success
            foreach (Node node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;
                    case NodeState.SUCCESSFUL:
                        return NodeState.SUCCESSFUL;
                        
                    case NodeState.FAILED:
                    default:
                        break;
                }
            }

            return NodeState.FAILED;
        }
    }
}