using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Sequence : Composite
    {
        // Used for maintaining position on RUNNING states and continuation from uninterruptable
        private int currentNodeIndex = 0;

        public Sequence(BehaviourTree behaviourTree, List<Node> childNodes, bool uninterruptable = false) : base(behaviourTree, childNodes, uninterruptable)
        { }

        protected override NodeState Evaluate()
        {            
            // Evaluate one node at a time, haulting at running or stopping at failure
            switch (UpdateChildNode(childNodes[currentNodeIndex]))
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;

                case NodeState.SUCCESSFUL:
                    currentNodeIndex++;

                    // IF finished all child nodes...
                    if(currentNodeIndex >= childNodes.Count)
                    {
                        OnReset();
                        return NodeState.SUCCESSFUL;
                    }
                    else
                    {
                        // Continue to evaluate other nodes
                        return Evaluate();
                    }

                case NodeState.FAILED:
                default:
                    OnReset();
                    return NodeState.FAILED;    // Node failed, we should fail
            }
        }

        protected override void OnReset()
        {
            base.OnReset();

            currentNodeIndex = 0;
        }
    }
}