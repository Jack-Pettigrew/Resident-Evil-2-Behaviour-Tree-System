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

        // VFX
        private ParticleSystem bulletHitParticleSystem;

        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        public void SetBulletHitParticleSystem(ParticleSystem bulletHitParticleSystem)
        {
            this.bulletHitParticleSystem = bulletHitParticleSystem;
        }

        public void Fire(int bulletDamage, float bulletSpeed)
        {
            if(!gameObject.activeInHierarchy) gameObject.SetActive(true);

            this.bulletDamage = bulletDamage;
            activeBullet = true;

            // Reset rigidbody velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other) {
            if(activeBullet)
            {                
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