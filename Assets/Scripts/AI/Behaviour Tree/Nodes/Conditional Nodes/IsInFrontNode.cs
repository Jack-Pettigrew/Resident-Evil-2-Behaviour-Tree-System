using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsInFrontNode<T> : Conditional
    {
        private readonly string blackboardKey;

        /// <summary>
        /// A Node to check whether the other Vector3/GameObject is in front of this AI or not.
        /// </summary>
        /// <param name="ai">The AI.</param>
        /// <param name="blackboardKey">The key of the BlackBoard variables.</param>
        public IsInFrontNode(IAIBehaviour ai, string blackboardKey) : base(ai)
        {
            this.blackboardKey = blackboardKey;
        }

        /// <summary>
        /// A Node to check whether the other Vector3/GameObject is in front of this AI or not.
        /// </summary>
        /// <param name="ai">The AI.</param>
        /// <param name="blackboardKey">The key of the BlackBoard variables.</param>
        /// <param name="trueNode">The Node to Branch to if true.</param>
        /// <param name="falseNode">The Node to Branch to if false.</param>
        public IsInFrontNode(IAIBehaviour ai, string blackboardKey, Node trueNode, Node falseNode) : base(trueNode, falseNode)
        {
            this.ai = ai;
            this.blackboardKey = blackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Transform sourceTransform = ai.GetAITransform();
            Vector3 targetPos;
            GetTargetVector3(ai.GetAIBlackboard().GetFromBlackboard<T>(blackboardKey), out targetPos);

            if (Vector3.Dot(sourceTransform.forward, targetPos - sourceTransform.position) > 0)
            {
                return NodeState.SUCCESSFUL;
            }

            return NodeState.FAILED;
        }

        // Trying to allow various Blackboard types to be used (GameObject, Transform or Vector3) as to not limit which types this class can be used for.
        // Generic specialisation overloading suggested by: https://stackoverflow.com/questions/982952/c-sharp-generics-and-type-checking
        // ...altough not on the other hand by: https://stackoverflow.com/questions/600978/how-to-do-template-specialization-in-c-sharp?noredirect=1&lq=1
        private void GetTargetVector3(T value, out Vector3 targetPos)
        {
            throw new System.Exception($"Unable to get Vector3 from Blackboard key: {blackboardKey}");
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