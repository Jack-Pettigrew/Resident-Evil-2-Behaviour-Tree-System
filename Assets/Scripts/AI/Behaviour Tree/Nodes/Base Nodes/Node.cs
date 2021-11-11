using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    [System.Serializable]
    public abstract class Node
    {
        protected readonly BehaviourTree behaviourTree;

        public Node(BehaviourTree behaviourTree)
        {
            this.behaviourTree = behaviourTree;
        }

        /// <summary>
        /// Evaluates the Node's logic.
        /// </summary>
        /// <returns>Node State result from evaluation logic.</returns>
        public abstract NodeState Evaluate();
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESSFUL,
        FAILED
    }
}