using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.AI.Sensors
{
    public class AIVision : AISensor
    {
        [Header("Components")]
        [SerializeField] private Transform visionTransform;
        
        [Header("Vision")]
        [SerializeField] private float fovAngle = 90.0f;
        [SerializeField] private float fovDistance = 2.0f;
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField] private LayerMask environmentLayerMask;

        private Collider[] colliders = new Collider[1];
        
        /// <summary>
        /// Senses whether a collider with the set layer mask can be seen.
        /// </summary>
        /// <returns>Whether a target has been found.</returns>
        public override bool Sense()
        {
            // Vicinity
            Physics.OverlapSphereNonAlloc(visionTransform.position, fovDistance, colliders, targetLayerMask, QueryTriggerInteraction.Ignore);
            foreach (var collider in colliders)
            {
                if (collider == null) continue;

                Vector3 targetDir = collider.transform.position - visionTransform.position;

                // Is player within view cone?
                if (Mathf.Abs(Vector3.Angle(visionTransform.forward, targetDir)) <= fovAngle)
                {
                    // Is view to player unobsctructed?
                    if (!Physics.Raycast(visionTransform.position, targetDir, fovDistance, environmentLayerMask, QueryTriggerInteraction.Ignore))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(visionTransform.position, fovDistance);
        }
    }
}
