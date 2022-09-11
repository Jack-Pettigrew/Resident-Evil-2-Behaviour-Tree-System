using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public abstract class LeafNode : Node
    {
        protected LeafNode(BehaviourTree behaviourTree, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
        }
    }
}
