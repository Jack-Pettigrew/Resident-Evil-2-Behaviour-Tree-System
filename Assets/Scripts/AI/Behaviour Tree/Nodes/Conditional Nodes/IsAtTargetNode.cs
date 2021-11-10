using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsAtTargetNode : Conditional
    {

        public IsAtTargetNode(Controllers.IAIBehaviour ai) : base(ai)
        {
        }

        protected override NodeState EvaluateConditional()
        {
            // hardcoded for testing
            Debug.Log("IsAtNode");
            return (ai.GetAIBlackboard().GetFromBlackboard<Transform>("Player").position - ai.GetAITransform().position).magnitude <= 1.5f ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }

}