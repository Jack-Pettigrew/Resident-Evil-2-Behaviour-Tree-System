using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core
{
    [RequireComponent(typeof(Collider))]
    public class WeakPoint : MonoBehaviour, IDamagable
    {
        [field: SerializeField] public bool IsDamagable { get; set; }

        [SerializeField, Min(1.0f)] private float stunMultiplyer = 1.0f;
        [SerializeField] private StunManager stunManager;

        private void Awake() {
            if(!stunManager)
            {
                Debug.LogWarning("No StunManager linked to this Weak Point.", this);
            }
        }

        public void TakeDamage(float damageAmount)
        {
            if(!IsDamagable) return;

            stunManager.Stun(damageAmount * stunMultiplyer);
        }
    }
}
