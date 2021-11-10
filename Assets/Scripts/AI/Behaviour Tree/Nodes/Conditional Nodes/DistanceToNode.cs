using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class DistanceToNode<T> : Conditional
    {
        private readonly string blackboardKeyA, blackboardKeyB;
        private readonly ConditionType distanceConditionType;
        private readonly float distanceThreshold;

        /// <summary>
        /// Distanace Node. Checks the distance between two GameObjects A to B using the condition type against distance. (Pure Conditional)
        /// </summary>
        /// <param name="ai">The AI Behaviour.</param>
        /// <param name="blackboardKeyA">The key of the first blackboard variable.</param>
        /// <param name="blackboardKeyB">The key of the second blackboard variable.</param>
        /// <param name="distanceConditionType">The conditional type for this calculation.</param>
        /// <param name="distanceThreshold">The value to check against.</param>
        /// <param name="areVariablesGameObjects">Whether the check is comparing GameObjects or Vector3s directly.</param>
        public DistanceToNode(IAIBehaviour ai, string blackboardKeyA, string blackboardKeyB, ConditionType distanceConditionType, float distanceThreshold, bool areVariablesGameObjects = false) : base(ai)
        {
            this.blackboardKeyA = blackboardKeyA;
            this.blackboardKeyB = blackboardKeyB;
            this.distanceConditionType = distanceConditionType;
            this.distanceThreshold = distanceThreshold;
        }

        /// <summary>
        /// Distanace Node. Checks the distance between two GameObjects A to B using the condition type against distance. (Branching Conditional)
        /// </summary>
        /// <param name="ai">The AI Behaviour.</param>
        /// <param name="blackboardKeyA">The key of the first blackboard variable.</param>
        /// <param name="blackboardKeyB">The key of the second blackboard variable.</param>
        /// <param name="distanceConditionType">The conditional type for this calculation.</param>
        /// <param name="distanceThreshold">The value to check against.</param>
        /// <param name="trueNode">The Node to branch to when True.</param>
        /// <param name="failNode">The Node to branch to when False.</param>
        /// <param name="areVariablesGameObjects">Whether the check is comparing GameObjects or Vector3s directly.</param>
        public DistanceToNode(IAIBehaviour ai, string blackboardKeyA, string blackboardKeyB, ConditionType distanceConditionType, float distanceThreshold, Node trueNode, Node failNode, bool areVariablesGameObjects = false) : base(trueNode, failNode)
        {
            this.ai = ai;
            this.blackboardKeyA = blackboardKeyA;
            this.blackboardKeyB = blackboardKeyB;
            this.distanceConditionType = distanceConditionType;
            this.distanceThreshold = distanceThreshold;
        }

        protected override NodeState EvaluateConditional()
        {
            Vector3 posA, posB;
            GetTargetVector3(ai.GetAIBlackboard().GetFromBlackboard<T>(blackboardKeyA), out posA);
            GetTargetVector3(ai.GetAIBlackboard().GetFromBlackboard<T>(blackboardKeyB), out posB);

            float dist = (posB - posA).magnitude;

            switch (distanceConditionType)
            {
                case ConditionType.GreaterThan:
                    return dist > distanceThreshold ? NodeState.SUCCESSFUL : NodeState.FAILED;

                case ConditionType.LessThan:
                    return dist < distanceThreshold ? NodeState.SUCCESSFUL : NodeState.FAILED;
                default:
                    return NodeState.FAILED;
            }
        }

        // Trying to allow various Blackboard types to be used (GameObject, Transform or Vector3) as to not limit which types this class can be used for.
        // Generic specialisation overloading suggested by: https://stackoverflow.com/questions/982952/c-sharp-generics-and-type-checking
        // ...altough not on the other hand by: https://stackoverflow.com/questions/600978/how-to-do-template-specialization-in-c-sharp?noredirect=1&lq=1
        private void GetTargetVector3(T value, out Vector3 targetPos)
        {
            throw new System.Exception($"Unable to get Vector3 from Blackboard key.");
        }

        private void GetTargetVector3(Vector3 vector3, out Vector3 targetPos)
        {
            targetPos = vector3;
        }

        private void GetTargetVector3(Transform transform, out Vector3 targetPos)
        {
            targetPos = transform.position;
        }

        private void GetTargetVector3(GameObject gameObject, out Vector3 targetPos)
        {
            targetPos = gameObject.transform.position;
        }
    }
}