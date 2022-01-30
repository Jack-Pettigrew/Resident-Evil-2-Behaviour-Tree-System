using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    
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

}
