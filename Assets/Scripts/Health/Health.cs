using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Core
{
    public class Health : MonoBehaviour, IDamagable, IKillable
    {
        [field: Header("Health")]
        [field: SerializeField] public bool IsDamagable { get; set; }

        [SerializeField] private float maxHealth = 100.0f;
        public float CurrentHealth { get; private set; }

        [Space(10)]
        [SerializeField] protected bool hasDamageCooldown = true;
        [SerializeField] protected float damageCooldown = 1.0f;

        public bool IsDead { get; private set; }
        public UnityEvent OnDeath;

        public virtual void TakeDamage(float damageAmount)
        {
            if (!IsDamagable || IsDead) return;

            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
            {
                Die();
            }
            else if (hasDamageCooldown)
            {
                StartCoroutine(DamageCooldown());
            }
        }

        public virtual IEnumerator DamageCooldown()
        {
            IsDamagable = false;

            yield return new WaitForSeconds(damageCooldown);

            IsDamagable = true;
        }

        public virtual void Die()
        {
            IsDead = true;
            OnDeath?.Invoke();
            StartCoroutine(CleanUp());
        }

        public virtual IEnumerator CleanUp()
        {
            yield return new WaitForSeconds(5.0f);
            Destroy(gameObject);
        }
    }
}