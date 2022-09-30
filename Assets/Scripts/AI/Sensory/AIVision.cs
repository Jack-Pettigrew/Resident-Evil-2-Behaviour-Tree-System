using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        private Collider sensedCollider = null;

        [Header("Events")]
        public UnityEvent<GameObject> OnObjectSensed;
        public UnityEvent<GameObject> OnSensedObjectLeft;

        /// <summary>
        /// Senses whether a collider with the set layer mask can be seen.
        /// </summary>
        /// <returns>Whether a target has been found.</returns>
        public override bool Sense()
        {
            // Vicinity
            // Check whether overlap actually updates the array, otherwise it hasn't found anything (array will always be initialised but not necessarily updated)
            int collidersFound = Physics.OverlapSphereNonAlloc(visionTransform.position, fovDistance, colliders, targetLayerMask, QueryTriggerInteraction.Ignore);
            
            if (collidersFound > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider == null) continue;

                    Vector3 targetDir = collider.transform.position - visionTransform.position;

                    // Is target within view cone?
                    if (Mathf.Abs(Vector3.Angle(visionTransform.forward, targetDir)) <= fovAngle)
                    {
                        // Is view to target unobsctructed?
                        if (!Physics.Raycast(visionTransform.position, targetDir, fovDistance, environmentLayerMask, QueryTriggerInteraction.Ignore))
                        {
                            sensedCollider = collider;
                            OnObjectSensed.Invoke(sensedCollider.gameObject);
                            return true;
                        }
                    }
                }
            }

            if (sensedCollider != null)
            {
                Debug.Log("Lost");
                OnSensedObjectLeft.Invoke(sensedCollider.gameObject);
                sensedCollider = null;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(visionTransform.position, fovDistance);
        }
    }
}
