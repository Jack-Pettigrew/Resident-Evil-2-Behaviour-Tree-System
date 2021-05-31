using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsInFront : Conditional
    {
        private readonly IAIBehaviour ai;
        private readonly string blackboardKey;

        /// <summary>
        /// A Node to check whether the other Vector3/GameObject is in front of this AI or not.
        /// </summary>
        /// <param name="ai">The AI.</param>
        /// <param name="blackboardKey">The key of the BlackBoard variables.</param>
        public IsInFront(IAIBehaviour ai, string blackboardKey) : base()
        {
            this.ai = ai;
            this.blackboardKey = blackboardKey;
        }

        /// <summary>
        /// A Node to check whether the other Vector3/GameObject is in front of this AI or not.
        /// </summary>
        /// <param name="ai">The AI.</param>
        /// <param name="blackboardKey">The key of the BlackBoard variables.</param>
        /// <param name="trueNode">The Node to Branch to if true.</param>
        /// <param name="falseNode">The Node to Branch to if false.</param>
        public IsInFront(IAIBehaviour ai, string blackboardKey, Node trueNode, Node falseNode) : base(trueNode, falseNode)
        {
            this.ai = ai;
            this.blackboardKey = blackboardKey;
        }

        protected override NodeState EvaluateConditional()
        {
            Transform sourceTransform = ai.GetAITransform();
            Vector3 destPosition = ai.GetAIBlackboard().GetFromBlackboard<Transform>(blackboardKey).position;

            if(Vector3.Dot(sourceTransform.forward, destPosition - sourceTransform.position) > 0)
            {
                return NodeState.SUCCESSFUL;
            }

            return NodeState.FAILED;
        }
    }

}