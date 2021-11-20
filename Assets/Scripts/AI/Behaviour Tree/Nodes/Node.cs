using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    [System.Serializable]
    public abstract class Node
    {
        // Behaviour Tree references
        protected readonly BehaviourTree behaviourTree;

        // Node Instance Variables
        public bool NodeRunning { private set; get; }

        public Node(BehaviourTree behaviourTree)
        {
            this.behaviourTree = behaviourTree;
        }

        /// <summary>
        /// The Update function for all Nodes. This is called by the Behaviour Tree each traversal.
        /// </summary>
        /// <returns></returns>
        public NodeState UpdateNode()
        {
            if(!OnStart())
            {
                return NodeState.FAILED;
            }

            NodeState result = Evaluate();

            if(!OnExit())
            {
                return NodeState.FAILED;
            }

            return result;
        }

        /* 
         * OnStart and OnExit functions may or may not be needed based on the current BT implementation
         * (heavily relying on the Blackboard for Node variables means nodes don't need to update local variables to match any changes)
        */
        protected virtual bool OnStart()
        {
            return true;
        }
        protected virtual bool OnExit()
        {
            return true;
        }

        /// <summary>
        /// Evaluates the Node's logic.
        /// </summary>
        /// <returns>Node State result from evaluation logic.</returns>
        protected abstract NodeState Evaluate();
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESSFUL,
        FAILED
    }
}