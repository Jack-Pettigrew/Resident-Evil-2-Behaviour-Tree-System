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
        private ParticleSystem bulletHitParticleSystem;

        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        public void SetBulletHitParticleSystem(ParticleSystem bulletHitParticleSystem)
        {
            this.bulletHitParticleSystem = bulletHitParticleSystem;
        }

        public void Fire(int bulletDamage, Vector3 firePosition, Vector3 direction, float bulletSpeed)
        {
            if(!gameObject.activeInHierarchy) gameObject.SetActive(true);

            activeBullet = true;
            this.bulletDamage = bulletDamage;

            // Reset rigidbody velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = firePosition;
            transform.forward = direction;

            rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other) {
            if(activeBullet)
            {
                Debug.Log($"Hit: {other.gameObject.name}");
                
                activeBullet = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Play bullet hit particle system
                if(bulletHitParticleSystem)
                {
                    bulletHitParticleSystem.transform.position = other.GetContact(0).point;
                    bulletHitParticleSystem.transform.forward = other.GetContact(0).normal;
                    bulletHitParticleSystem.Play();
                }

                IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
                if(damagable != null)
                {
                    damagable.TakeDamage(bulletDamage);
                } 
            }
        }    
    }
}