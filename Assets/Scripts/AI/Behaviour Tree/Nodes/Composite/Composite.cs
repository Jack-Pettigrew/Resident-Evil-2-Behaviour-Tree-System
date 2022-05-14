using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public abstract class Composite : Node
    {
        /// <summary>
        /// All the connected child nodes this Composite branches down to.
        /// </summary>
        protected List<Node> childNodes;

        /// <summary>
        /// List of Service nodes to be executed before Composite evaluation.
        /// </summary>
        protected List<Service> serviceNodes;

        public Composite(BehaviourTree behaviourTree, List<Node> childNodes) : base(behaviourTree)
        {
            this.childNodes = childNodes;
            this.serviceNodes = null;
        }

        public Composite(BehaviourTree behaviourTree, List<Node> childNodes, List<Service> serviceNodes) : base(behaviourTree)
        {
            this.childNodes = childNodes;
            this.serviceNodes = serviceNodes;
        }

        protected override bool OnStart()
        {
            // Service Nodes
            if (serviceNodes?.Count > 0)
            {
                foreach (var node in serviceNodes)
                {
                    if (node.UpdateNode() == NodeState.FAILED)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected override bool OnExit(NodeState nodeState)
        {
            switch (nodeState)
            {
                case NodeState.FAILED:
                    break;

                default:
                    behaviourTree.LogBranchNode(this);
                    break;
            }

            return true;
        }
    }
}