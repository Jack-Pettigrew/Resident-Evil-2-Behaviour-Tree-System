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
        private List<Node> previousBranch = new List<Node>();
        private List<Node> currentBranch = new List<Node>();
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

            HandleBranchChanges();
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

                break;

                default:
                // Account for whether it failed
                break;
            }

            if(priorityNodes.Peek().State == NodeState.SUCCESSFUL)
            {
                priorityNodes.Dequeue();
            }

        }


        /// <summary>
        /// Handles difference between current execution branch and the previous one.
        /// </summary>
        private void HandleBranchChanges()
        {
            if (currentBranch.Count > 0)
            {
                int i;
                for (i = 0; i < previousBranch.Count; i++)
                {
                    // 0. Break if we reach more nodes than the current branch has
                    if (i > currentBranch.Count - 1) break;

                    // 1. Check if Node at index i in current branch isn't the same as that in the previous branch
                    if (currentBranch[i] != previousBranch[i])
                    {
                        // 1.1 If not, check if previous node status is currently running and add to be reseted
                        if (previousBranch[i].State == NodeState.RUNNING)
                        {
                            branchNodesToReset.Add(previousBranch[i]);
                        }
                    }
                }

                // 2. If previousBranch was longer than current, remaining add RUNNING nodes to reset
                if (i < previousBranch.Count - 1)
                {
                    for (i = i; i < previousBranch.Count; i++)
                    {
                        // 2.1 Check if node was running and add to resetable
                        if (previousBranch[i].State == NodeState.RUNNING)
                        {
                            branchNodesToReset.Add(previousBranch[i]);
                        }
                    }
                }

                // 3. If we have nodes to reset, reset them
                if (branchNodesToReset.Count > 0)
                {
                    foreach (Node node in branchNodesToReset)
                    {
                        node.OnReset();
                    }

                    branchNodesToReset.Clear();
                }

                // 3. Update branch history
                previousBranch = new List<Node>(currentBranch);
                currentBranch.Clear();
            }
        }

        /// <summary>
        /// Logs the Node as being executed this current execution branch.
        /// </summary>
        /// <param name="node">The node reached.</param>
        public void LogReachedNode(Node node)
        {
            if(!currentBranch.Contains(node))
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