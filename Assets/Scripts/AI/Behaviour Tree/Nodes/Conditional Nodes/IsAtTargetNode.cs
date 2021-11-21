using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsAtTargetNode<T> : Conditional where T : MonoBehaviour
    {
        private readonly string targetBlackboardKey;

        public IsAtTargetNode(BehaviourTree behaviourTree, string targetBlackboardKey) : base(behaviourTree)
        {
            this.targetBlackboardKey = targetBlackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            return (behaviourTree.Blackboard.GetFromBlackboard<T>(targetBlackboardKey).transform.position - behaviourTree.ai.GetAITransform().position).magnitude <= 1.5f ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }

}