using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsAtPoint : Conditional
    {
        private readonly string pointBlackboardKey;
        private readonly float distanceThreshold;
        
        public IsAtPoint(BehaviourTree behaviourTree, string pointBlackboardKey, float disatnceThreshold) : base(behaviourTree)
        { 
            this.pointBlackboardKey = pointBlackboardKey;
            this.distanceThreshold = disatnceThreshold;
        }

        protected override NodeState EvaluateConditional()
        {
            return Vector3.Distance(behaviourTree.Blackboard.GetFromBlackboard<Vector3>(pointBlackboardKey), behaviourTree.ai.GetAITransform().position) <= distanceThreshold ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
