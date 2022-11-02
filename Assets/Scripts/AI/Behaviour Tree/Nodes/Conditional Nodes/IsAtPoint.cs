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
        }

        protected override NodeState EvaluateConditional()
        {
            return Vector3.Distance(behaviourTree.ai.GetAITransform().position, behaviourTree.Blackboard.GetFromBlackboard<Vector3>(pointBlackboardKey)) <= distanceThreshold ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
