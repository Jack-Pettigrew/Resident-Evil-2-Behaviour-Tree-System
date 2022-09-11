using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class BehaviourTree
    {
        private Node rootNode = null;
        public Blackboard Blackboard { private set; get; }

        // BRANCH TRACKING
        private HashSet<Node> previousBranch = new HashSet<Node>();
        private HashSet<Node> currentBranch = new HashSet<Node>();
        private List<Node> branchNodesToReset = new List<Node>();

        // Priority Nodes
        private Queue<Node> priorityNodes = new Queue<Node>();

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
            if (priorityNodes.Count > 0)
            {
                UpdatePriorityNodes();
                return;
            }

            UpdateTree();

            HandleBranchChange();
        }

        /// <summary>
        /// Updates the Behaviour Tree.
        /// </summary>
        private void UpdateTree()
        {
            // Currently evaluating entire tree each tick
            switch (rootNode.UpdateNode())
            {
                case NodeState.FAILED:
                    string nodePath = string.Empty;

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (Node node in currentBranch)
                    {
                        sb.Append(node.GetType().ToString() + '\n');
                    }

                    Debug.LogWarning("Behaviour Tree Failure reported:" + sb.ToString());
                    break;

                case NodeState.RUNNING:
                    break;

                default:
                    break;
            }

            /**
            * TODO:
            * Handle different Node States diferently
            * - Failed
            * - Reseted (needs accounting for within each node too)
            * 
            * Optimisation of Tree traversal and node execution
            */

        }

        private void UpdatePriorityNodes()
        {
            priorityNodes.Peek().UpdateNode();

            switch (priorityNodes.Peek().State)
            {
                case NodeState.SUCCESSFUL:
                case NodeState.FAILED:
                    priorityNodes.Dequeue();
                    break;

                default:
                    // Account for whether it failed
                    break;
            }
        }

        /// <summary>
        /// Handles difference between current execution branch and the previous one, requesting outdated Nodes reset.
        /// The logic indirectly controls whether the behaviour tree should evaluate every single node the next tick.
        /// </summary>
        private void HandleBranchChange()
        {
            // Reset nodes no longer in the current branch
            foreach (Node node in previousBranch)
            {
                // * Commenting out IF statement makes Behaviour Tree reset all branched Nodes regardless of whether they
                // * were in the current branch or not
                // // if (!currentBranch.Contains(node))
                // // {
                    node.Reset();
                // // }
            }

            previousBranch = new HashSet<Node>(currentBranch);
            currentBranch.Clear();
        }

        /// <summary>
        /// Logs the Node as being executed this current execution branch.
        /// </summary>
        /// <param name="node">The node reached.</param>
        public void LogReachedNode(Node node)
        {
            if (!currentBranch.Contains(node))
            {
                currentBranch.Add(node);
            }

            if (node.IsUninterruptable && !priorityNodes.Contains(node))
            {
                priorityNodes.Enqueue(node);
            }
        }
    }
}