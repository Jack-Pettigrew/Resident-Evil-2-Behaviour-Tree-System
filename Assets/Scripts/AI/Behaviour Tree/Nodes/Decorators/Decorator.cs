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
        // Child Node to repeat
        protected Node childNode;

        public Decorator(BehaviourTree behaviourTree, Node childNode) : base(behaviourTree)
        {
            this.childNode = childNode;
        }
    }
}