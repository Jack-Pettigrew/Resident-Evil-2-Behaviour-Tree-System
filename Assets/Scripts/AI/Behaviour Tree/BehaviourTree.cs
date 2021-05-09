using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class BehaviourTree
    {
        // Change this to a custom BehaviourTreeAsset later (it'll have all the nodes in it - or will at least be able to generate one from the asset)
        private Node rootNode = null;

        // Behaviour Tree's Blackboard
        public Blackboard Blackboard { private set; get; }

        public BehaviourTree()
        {
            Blackboard = new Blackboard();
        }

        public void SetBehaviourTree(Node rootNode)
        {
            this.rootNode = rootNode;
        }

        public void EvaluateTree()
        {
            // Currently evaluating entire tree each tick
            NodeState result = rootNode.Evaluate();

            switch (result)
            {
                case NodeState.FAILED:
                    Debug.LogWarning("Behaviour Tree Failure reported.");
                    break;
                default:
                    break;
            }

            // TO DO:
            // Bubbled up returned NodeState handling (mainly if failed or running).
        }
    }
}