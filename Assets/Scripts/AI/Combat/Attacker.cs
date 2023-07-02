using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Combat
{
    public class Attacker : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform attackOriginTransform;
        [SerializeField] private Vector3 originOffset = Vector3.zero;

        [Header("Damage")]
        [SerializeField] private LayerMask attackableLayer;
        [SerializeField] private float attackRadius = 1.0f;
        [Tooltip("The amount of damage an object will recieve.")]
        [SerializeField] private int damageAmount;

        private void Awake()
        {
            if (attackOriginTransform == null)
            {
                Debug.LogWarning($"{gameObject.name} attack collider has no reference to a Collider!");
            }
        }

        public void Attack()
        {
            Vector3 attackPosition = attackOriginTransform.position + attackOriginTransform.forward * originOffset.z + attackOriginTransform.right * originOffset.x + attackOriginTransform.up * originOffset.y;
            Collider[] colliders = Physics.OverlapSphere(attackPosition, attackRadius, attackableLayer, QueryTriggerInteraction.Ignore);

            foreach (var collider in colliders)
            {
                collider.GetComponent<IDamagable>().TakeDamage(damageAmount);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackOriginTransform)
            {
                Vector3 attackPosition = attackOriginTransform.position + attackOriginTransform.forward * originOffset.z + attackOriginTransform.right * originOffset.x + attackOriginTransform.up * originOffset.y;
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawSphere(attackPosition, attackRadius);
            }
        }
    }
}
 