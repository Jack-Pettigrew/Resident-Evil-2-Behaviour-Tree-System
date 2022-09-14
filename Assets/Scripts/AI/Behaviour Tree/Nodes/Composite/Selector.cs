using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Selector : Composite
    {
        // Used for maintaining position on RUNNING states and continuation from uninterruptable
        private int currentNodeIndex = 0;

        public Selector(BehaviourTree behaviourTree, List<Node> childNodes, bool uninterruptable = false) : base(behaviourTree, childNodes, uninterruptable)
        { }

        protected override NodeState Evaluate()
        {            
            // Process all until Success
            for(; currentNodeIndex < childNodes.Count; currentNodeIndex++)
            {
                switch (UpdateChildNode(childNodes[currentNodeIndex]))
                {
                    case NodeState.RUNNING:
                        OnReset();
                        return NodeState.RUNNING;
                        
                    case NodeState.SUCCESSFUL:
                        OnReset();
                        return NodeState.SUCCESSFUL;

                    case NodeState.FAILED:
                    default:
                        break;
                }
            }

            OnReset();
            return NodeState.FAILED;
        }

        protected override void OnReset()
        {
            currentNodeIndex = 0;
        }
    }
}