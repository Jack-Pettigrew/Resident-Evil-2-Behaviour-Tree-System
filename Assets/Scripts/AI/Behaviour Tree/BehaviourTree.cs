using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class BehaviourTree
    {
        private Node rootNode = null;
        public Blackboard Blackboard { private set; get; }

        // BRANCH TRACKING
        private List<Composite> previousBranch = new List<Composite>(5);
        private List<Composite> currentBranch = new List<Composite>(5);

        // COMPONENTS
        public IAIBehaviour ai { private set; get; }

        public BehaviourTree(IAIBehaviour ai)
        {
            this.ai = ai;
            Blackboard = new Blackboard();
        }

        public void SetBehaviourTree(Node rootNode)
        {
            this.rootNode = rootNode;
        }

        public void EvaluateTree()
        {
            // Currently evaluating entire tree each tick
            switch (rootNode.UpdateNode())
            {
                case NodeState.FAILED:
                    Debug.LogWarning("Behaviour Tree Failure reported - define something to do on complete branch failure.");
                    break;
                default:
                    break;
            }

            HandleLoggedNodes();

            /* TO DO:
            * Handle different Node States diferently
            * - Failed
            * - Interupted (needs accounting for within each node too)
            * 
            * Optimisation of Tree traversal and node execution
            */
        }

        /// <summary>
        /// Handles difference between current execution branch and the previous one.
        /// </summary>
        private void HandleLoggedNodes()
        {
            if (previousBranch.Count > 0)
            {
                List<Composite> oldBranchNodes = previousBranch.Except(currentBranch).ToList();

                if (oldBranchNodes.Count > 0)
                {
                    foreach (Composite node in oldBranchNodes)
                    {
                        node.Interupt();
                    }
                }
            }

            previousBranch.Clear();
            previousBranch.AddRange(currentBranch);
            currentBranch.Clear();
        }

        /// <summary>
        /// Logs the Node as having been executed this execution branch.
        /// </summary>
        /// <param name="node"></param>
        public void LogBranchNode(Composite node)
        {
            currentBranch.Add(node);
        }
    }
}