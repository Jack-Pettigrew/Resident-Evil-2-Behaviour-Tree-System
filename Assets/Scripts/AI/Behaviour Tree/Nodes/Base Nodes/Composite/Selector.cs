using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Selector : Node
    {
        protected List<Node> nodes;

        public Selector(BehaviourTree behaviourTree, List<Node> nodes) : base(behaviourTree)
        {
            this.nodes = new List<Node>(nodes);
        }

        protected override NodeState Evaluate()
        {
            // Process all until Success
            foreach (Node node in nodes)
            {
                switch (node.UpdateNode())
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