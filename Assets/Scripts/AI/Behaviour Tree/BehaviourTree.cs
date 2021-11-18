using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class BehaviourTree
    {
        public IAIBehaviour ai { private set; get; }
        private Node rootNode = null;
        public Blackboard Blackboard { private set; get; }

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
            NodeState result = rootNode.UpdateNode();

            switch (result)
            {
                case NodeState.FAILED:
                    Debug.LogWarning("Behaviour Tree Failure reported.");
                    break;
                default:
                    break;
            }

            /* TO DO:
            * Handle different Node States diferently
            * - Failed
            * - Interupted (needs accounting for within each node too)
            */

        }
    }
}