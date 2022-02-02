using System.Collections;

namespace DD.Core
{
    public interface IDamagable
    {
        bool IsDamagable { get; set; }
        
        void TakeDamage(float damageAmount);
        IEnumerator DamageCooldown();
    }
}
