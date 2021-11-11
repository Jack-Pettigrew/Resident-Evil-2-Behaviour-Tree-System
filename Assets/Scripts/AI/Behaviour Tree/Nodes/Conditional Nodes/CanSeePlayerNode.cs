using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class CanSeePlayerNode : Conditional
    {
        private float fovAngle = 45.0f;
        private float fovRange = 2.0f;
        private LayerMask layerMask = new LayerMask();

        public CanSeePlayerNode(IAIBehaviour ai, float fovAngle, float fovRange, LayerMask layerMask) : base(ai)
        {
            this.fovAngle = fovAngle;
            this.fovRange = fovRange;
            this.layerMask = layerMask;
        }

        protected override NodeState EvaluateConditional()
        {
            Debug.Log("CanSeePlayer");
            Collider[] colliders = Physics.OverlapSphere(ai.GetAITransform().position, fovRange, layerMask, QueryTriggerInteraction.Ignore);

            foreach (var collider in colliders)
            {
                Vector3 targetDir = collider.transform.position - ai.GetAITransform().position;

                if (Mathf.Abs(Vector3.Angle(ai.GetAITransform().forward, targetDir)) <= fovAngle) // Can See Player
                {
                    return NodeState.SUCCESSFUL;
                }
            }

            return NodeState.FAILED;
        }
    }
}