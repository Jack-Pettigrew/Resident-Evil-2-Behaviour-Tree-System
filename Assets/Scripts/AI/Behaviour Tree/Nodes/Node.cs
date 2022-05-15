using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    [System.Serializable]
    public abstract class Node : IInteruptable
    {
        // Node References
        protected readonly BehaviourTree behaviourTree;

        // Node Status
        public NodeState State { private set; get; }

        public Node(BehaviourTree behaviourTree)
        {
            this.behaviourTree = behaviourTree;
        }

        /// <summary>
        /// The node start logic.
        /// </summary>
        /// <returns>Success?</returns>
        protected virtual bool NodeStart()
        {                        
            // Log Node as reached
            behaviourTree.LogReachedNode(this);
            
            return OnStart();
        }

        /// <summary>
        /// The node exit logic.
        /// </summary>
        /// <returns>Success?</returns>
        private bool NodeExit()
        {
            return OnExit();
        }

        /// <summary>
        /// The Update function for all Nodes. This is called by the Behaviour Tree each traversal.
        /// </summary>
        /// <returns></returns>
        public NodeState UpdateNode()
        {
            if(!NodeStart())
            {
                return State = NodeState.FAILED;
            }

            State = Evaluate();            

            if(!NodeExit())
            {
                return State = NodeState.FAILED;
            }

            return State;
        }

        /// <summary>
        /// Custom node start logic.
        /// </summary>
        /// <returns>Success?</returns>
        protected virtual bool OnStart()
        {
            return true;
        }

        /// <summary>
        /// Custom node exit logic.
        /// </summary>
        /// <returns>Success?</returns>
        protected virtual bool OnExit()
        {
            return true;
        }

        /// <summary>
        /// Evaluates the Node's logic.
        /// </summary>
        /// <returns>Node State result from evaluation logic.</returns>
        protected abstract NodeState Evaluate();

        /// <summary>
        /// <para>Called when running node is no longer a part the currently executing branch. Contains all necessary for clean up logic.</para>
        /// When overriding ensure base logic is also called.
        /// </summary>
        public virtual void OnInterupt()
        {
            NodeExit();
            State = NodeState.NONE;
        }
    }

    public enum NodeState
    {
        NONE,
        RUNNING,
        SUCCESSFUL,
        FAILED
    }
}