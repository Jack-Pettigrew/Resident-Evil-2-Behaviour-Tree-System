using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class DistanceToNode : Conditional
    {
        private readonly IAIBehaviour ai;
        private readonly string blackboardKeyA, blackboardKeyB;
        private readonly DistanceConditionType distanceConditionType;
        private readonly float distanceThreshold;
        private readonly bool areVariablesGameObjects;

        /// <summary>
        /// Distanace Node. Checks the distance between two GameObjects A to B using the condition type against distance.
        /// </summary>
        /// <param name="ai">The AI Behaviour.</param>
        /// <param name="blackboardKeyA">The key of the first blackboard variable.</param>
        /// <param name="blackboardKeyB">The key of the second blackboard variable.</param>
        /// <param name="distanceConditionType">The conditional type for this calculation.</param>
        /// <param name="distanceThreshold">The value to check against.</param>
        /// <param name="areGameObjects">Whether the check is comparing GameObjects or Vector3s directly.</param>
        public DistanceToNode(IAIBehaviour ai, string blackboardKeyA, string blackboardKeyB, DistanceConditionType distanceConditionType, float distanceThreshold, bool areVariablesGameObjects = false) : base()
        {
            this.ai = ai;
            this.blackboardKeyA = blackboardKeyA;
            this.blackboardKeyB = blackboardKeyB;
            this.distanceConditionType = distanceConditionType;
            this.distanceThreshold = distanceThreshold;
            this.areVariablesGameObjects = areVariablesGameObjects;
        }
        public DistanceToNode(IAIBehaviour ai, string blackboardKeyA, string blackboardKeyB, DistanceConditionType distanceConditionType, float distanceThreshold, Node trueNode, Node failNode, bool areVariablesGameObjects = false) : base(trueNode, failNode)
        {
            this.ai = ai;
            this.blackboardKeyA = blackboardKeyA;
            this.blackboardKeyB = blackboardKeyB;
            this.distanceConditionType = distanceConditionType;
            this.distanceThreshold = distanceThreshold;
            this.areVariablesGameObjects = areVariablesGameObjects;
        }

        protected override NodeState EvaluateConditional()
        {
            float dist;

            if (!areVariablesGameObjects)
            {
                dist = (ai.GetAIBlackboard().GetFromBlackboard<Vector3>(blackboardKeyB) - ai.GetAIBlackboard().GetFromBlackboard<Vector3>(blackboardKeyA)).magnitude;
            }
            else
            {
                dist = (ai.GetAIBlackboard().GetFromBlackboard<GameObject>(blackboardKeyB).transform.position - ai.GetAIBlackboard().GetFromBlackboard<GameObject>(blackboardKeyA).transform.position).magnitude;
            }

            switch (distanceConditionType)
            {
                case DistanceConditionType.GreaterThan:
                    return dist > distanceThreshold ? NodeState.SUCCESSFUL : NodeState.FAILED;

                case DistanceConditionType.LessThan:
                    return dist < distanceThreshold ? NodeState.SUCCESSFUL : NodeState.FAILED;
                default:
                    return NodeState.FAILED;
            }
        }
    }

    public enum DistanceConditionType { GreaterThan, LessThan}
}