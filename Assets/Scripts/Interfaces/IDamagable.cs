using System.Collections;

namespace DD.Core
{
    public interface IDamagable
    {
        bool IsDamagable { get; }
        
        void TakeDamage(float damageAmount);
        IEnumerator DamageCooldown();
    }
}
