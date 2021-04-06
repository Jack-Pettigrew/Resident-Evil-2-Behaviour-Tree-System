using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class CanSeePlayerNode : Conditional
    {
        private Transform transform = null;
        private float fovAngle = 45.0f;
        private float fovRange = 2.0f;
        private LayerMask layerMask = new LayerMask();

        public CanSeePlayerNode(Node trueNode, Node falseNode, Transform transform, float fovAngle, float fovRange)
        {
            this.trueNode = trueNode;
            this.falseNode = falseNode;

            this.transform = transform;
            this.fovAngle = fovAngle;
            this.fovRange = fovRange;
            layerMask = LayerMask.NameToLayer("Player");
        }

        public CanSeePlayerNode(Node trueNode, Node falseNode, Transform transform, float fovAngle, float fovRange, LayerMask layerMask)
        {
            this.trueNode = trueNode;
            this.falseNode = falseNode;

            this.transform = transform;
            this.fovAngle = fovAngle;
            this.fovRange = fovRange;
            this.layerMask = layerMask;
        }

        protected override Node EvaluateConditional()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, fovRange, layerMask, QueryTriggerInteraction.Ignore);

            foreach (var collider in colliders)
            {
                Vector3 targetDir = collider.transform.position - transform.position;

                if (Mathf.Abs(Vector3.Angle(transform.forward, targetDir)) <= fovAngle) // Can See Player
                {
                    return trueNode;
                }
            }

            return falseNode;
        }
    }
}