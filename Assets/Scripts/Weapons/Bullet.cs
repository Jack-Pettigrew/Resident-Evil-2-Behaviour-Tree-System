using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        private int bulletDamage;
        private bool activeBullet = false;
        private Rigidbody rb;

        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        public void Fire(int bulletDamage, Vector3 firePosition, Vector3 direction, float bulletSpeed)
        {
            if(!gameObject.activeInHierarchy) gameObject.SetActive(true);

            activeBullet = true;
            this.bulletDamage = bulletDamage;
            transform.position = firePosition;
            transform.forward = direction;

            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other) {
            if(activeBullet)
            {
                activeBullet = false;

                Debug.Log($"Hit: {other.gameObject.name}");

                IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
                if(damagable != null)
                {
                    damagable.TakeDamage(bulletDamage);
                } 
            }
        }    
    }
}