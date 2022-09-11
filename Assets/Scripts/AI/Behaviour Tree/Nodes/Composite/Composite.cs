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

        public Composite(BehaviourTree behaviourTree, List<Node> childNodes, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
            this.childNodes = childNodes;
            this.serviceNodes = null;
        }

        public Composite(BehaviourTree behaviourTree, List<Node> childNodes, List<Service> serviceNodes, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
            this.childNodes = childNodes;
            this.serviceNodes = serviceNodes;
        }

        protected override bool OnStart()
        {            
            // Evaluate Service Nodes before composite updates
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
    }
}