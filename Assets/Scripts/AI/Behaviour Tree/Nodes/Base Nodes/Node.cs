using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    [System.Serializable]
    public class Node
    {
        private BehaviourTree behaviourTree;

        public Node(BehaviourTree tree)
        {
            behaviourTree = tree;
        }

        public virtual NodeState Evaluate()
        {
            return NodeState.FAILED;
        }
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESSFUL,
        FAILED
    }
}