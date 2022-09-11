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

        // Nodes that are running are being reset and therefor cannot continue the tree path from where they left off


        /// <summary>
        /// Handles difference between current execution branch and the previous one.
        /// </summary>
        private void HandleBranchChanges()
        {
            // All nodes need to be reset
            // Because of this, Running nodes lose track of which node they were on previously causing broken behaviour
            // before refactor, we only reset nodes that were left running on a different branch to the new one

            // change branch variables to HashSets and do simple Hashset.Contains() to check whether node is being run?
            // if previousBranch node is not in currentBranch node && reset node status

            if (currentBranch.Count > 0)
            {
                foreach (Node node in previousBranch)
                {
                    if (!currentBranch.Contains(node))
                    {
                        Debug.Log($"{node.ToString()} Reset");
                        node.Reset();
                    }
                }
            }

            previousBranch = new HashSet<Node>(currentBranch);
            currentBranch.Clear();


            // if (currentBranch.Count > 0)
            // {
            //     // 1. Compare as many nodes as we can within the previous branch against the current branch
            //     int i;
            //     for (i = 0; i < previousBranch.Count; i++)
            //     {
            //         // 1.2. Break if index i reaches more nodes than the current branch has
            //         if (i > currentBranch.Count - 1) break;

            //         // 1.3. Check if Node at index i in current branch is NOT the same as that in the previous branch
            //         if (currentBranch[i] != previousBranch[i])
            //         {
            //             // // 1.1 If not, check if previous node status is currently running and add to be reseted
            //             // // if (previousBranch[i].State == NodeState.RUNNING)
            //             // // {
            //                 branchNodesToReset.Add(previousBranch[i]);
            //             // // }
            //         }
            //     }

            //     // 2. If previousBranch was longer than current, add remaining nodes to reset
            //     if (i < previousBranch.Count - 1)
            //     {
            //         for (; i < previousBranch.Count; i++)
            //         {
            //             // // 2.1 Check if node was running and add to resetable
            //             // // if (previousBranch[i].State == NodeState.RUNNING)
            //             // // {
            //                 branchNodesToReset.Add(previousBranch[i]);
            //             // // }
            //         }
            //     }

            //     // 3. If we have nodes to reset, reset them
            //     if (branchNodesToReset.Count > 0)
            //     {
            //         foreach (Node node in branchNodesToReset)
            //         {
            //             node.OnReset();
            //         }

            //         branchNodesToReset.Clear();
            //     }

            //     // 3. Update branch history
            //     previousBranch = new List<Node>(currentBranch);
            //     currentBranch.Clear();
            // }
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