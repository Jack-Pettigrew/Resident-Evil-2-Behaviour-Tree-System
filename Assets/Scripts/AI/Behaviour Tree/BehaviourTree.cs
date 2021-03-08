using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class BehaviourTree
    {
        // Change this to a custom BehaviourTreeAsset later (it'll have all the nodes in it - or will at least be able to generate one from the asset)
        private Node rootNode = null;

        // Blackboard Solution Here

        public void SetBehaviourTree(Node rootNode)
        {
            this.rootNode = rootNode;
        }

        public void EvaluateTree()
        {
            rootNode?.Evaluate();
        }
    }
}