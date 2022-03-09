using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Core
{
    public class Health : MonoBehaviour, IDamagable, IKillable
    {
        [field: Header("Health")]
        [field: SerializeField] public bool IsDamagable { get; set; }

        [SerializeField, Min(0)] private float maxHealth = 100.0f;
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        [Space(10)]
        [SerializeField] protected bool hasDamageCooldown = true;
        [SerializeField] protected float damageCooldown = 1.0f;

        [Space(10)]

        public UnityEvent<float> OnHealthHealed;
        public UnityEvent<float> OnDamageTaken;
        public UnityEvent OnDeath;

        private void Awake() {
            CurrentHealth = maxHealth;
        }

        public void Heal(float healAmount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth += healAmount, maxHealth);
            OnHealthHealed?.Invoke(healAmount);
        }

        public virtual void TakeDamage(float damageAmount)
        {            
            if (!IsDamagable || IsDead) return;

            CurrentHealth -= damageAmount;
            OnDamageTaken?.Invoke(damageAmount);

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
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }
    }
}
