using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Collider attackCollider;
    [Header("Damage")]
    [Tooltip("The amount of damage an object will recieve.")]
    [SerializeField] private int damageAmount;
    
    private void Awake() {
        attackCollider.enabled = false;

        if(attackCollider == null)
        { 
            Debug.LogWarning($"{gameObject.name} attack collider has no reference to a Collider!");
        }
    }
    
    public void EnableCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision other) {
        other.gameObject.GetComponent<IDamagable>().TakeDamage(damageAmount);
    }
}
