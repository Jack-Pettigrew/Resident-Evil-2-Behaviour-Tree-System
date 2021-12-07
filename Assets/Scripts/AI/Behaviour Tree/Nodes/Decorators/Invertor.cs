using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Invertor : Decorator
    {
        public Invertor(BehaviourTree behaviourTree, Node childNode) : base(behaviourTree, childNode)
        { }

        protected override NodeState Evaluate()
        {
            switch (childNode.UpdateNode())
            {
                case NodeState.FAILED:
                    return NodeState.SUCCESSFUL;

                case NodeState.RUNNING:
                    return NodeState.RUNNING;

                case NodeState.SUCCESSFUL:
                default:
                    return NodeState.FAILED;
            }
        }
    }
}