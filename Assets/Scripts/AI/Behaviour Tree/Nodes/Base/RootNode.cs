using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

namespace DD.AI.BehaviourTreeSystem
{
    public class RootNode : Node
    {
        private Node childNode;

        public RootNode(BehaviourTree behaviourTree, Node childNode) : base(behaviourTree, false)
        {
            this.childNode = childNode;
        }

        protected override NodeState Evaluate()
        {
            return childNode.UpdateNode();
        }
    }
}