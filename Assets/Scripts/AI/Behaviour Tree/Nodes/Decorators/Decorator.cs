using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// Nodes that alter the behaviour of the branch with varying special logic.
    /// </summary>
    public abstract class Decorator : Node
    {
        // Child Node of the decorator
        protected Node childNode;

        public Decorator(BehaviourTree behaviourTree, Node childNode, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
            this.childNode = childNode;
        }
    }
}