using System.Collections;

public interface IDamagable
{
    bool IsDamagable{get;}
    bool TakeDamage(float damageAmount);
    IEnumerator DamageCooldown(float cooldownTime);
}
