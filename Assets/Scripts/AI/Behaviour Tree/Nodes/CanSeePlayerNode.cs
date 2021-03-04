using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTree
{
    public class CanSeePlayerNode : Node
    {
        private Transform transform = null;
        private float fovAngle = 45.0f;
        private float fovRange = 2.0f;
        private LayerMask layerMask = new LayerMask();

        public CanSeePlayerNode(Transform transform, float fovAngle, float fovRange)
        {
            this.transform = transform;
            this.fovAngle = fovAngle;
            this.fovRange = fovRange;
            this.layerMask = LayerMask.NameToLayer("Player");
        }

        public CanSeePlayerNode(Transform transform, float fovAngle, float fovRange, LayerMask layerMask)
        {
            this.transform = transform;
            this.fovAngle = fovAngle;
            this.fovRange = fovRange;
            this.layerMask = layerMask;
        }

        public override NodeState Evaluate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, fovRange, layerMask, QueryTriggerInteraction.Ignore);

            foreach (var collider in colliders)
            {
                Vector3 targetDir = collider.transform.position - transform.position;

                if (Mathf.Abs(Vector3.Angle(transform.forward, targetDir)) <= fovAngle) // Can See Player
                {
                    Debug.Log("Can See");
                    return NodeState.SUCCESSFUL;
                }
            }

            Debug.Log("Can't See");
            return NodeState.FAILED;
        }
    }
}