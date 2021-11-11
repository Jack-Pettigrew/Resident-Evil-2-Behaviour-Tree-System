using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsAtTargetNode : Conditional
    {
        public IsAtTargetNode(BehaviourTree behaviourTree) : base(behaviourTree)
        {
        }

        protected override NodeState EvaluateConditional()
        {
            // hardcoded for testing
            Debug.Log("IsAtNode");
            return (behaviourTree.Blackboard.GetFromBlackboard<Transform>("Player").position - behaviourTree.ai.GetAITransform().position).magnitude <= 1.5f ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }

}