using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsAtTarget<T> : Conditional where T : Component
    {
        private readonly string targetBlackboardKey;
        private float distanceThreshold;

        public IsAtTarget(BehaviourTree behaviourTree, string targetBlackboardKey, float distanceThreshold) : base(behaviourTree)
        {
            this.targetBlackboardKey = targetBlackboardKey;
            this.distanceThreshold = distanceThreshold;
        }

        protected override NodeState EvaluateConditional()
        {
            return (Vector3.Distance(behaviourTree.Blackboard.GetFromBlackboard<T>(targetBlackboardKey).transform.position, behaviourTree.ai.GetAITransform().position) <= distanceThreshold) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }

}