using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Combat
{
    public class Gun : Weapon
    {
        [field: Header("Ammo")]
        [field: SerializeField] public int MaxAmmoCapacity { private set; get; }
        public int CurrentAmmo { private set; get; }
        public bool IsReloading { private set; get; }
        [SerializeField] private float reloadTime = 1.0f;

        // Firing
        [Header("Firing")]
        [SerializeField] private Transform bulletOrigin;
        [SerializeField] private GameObject bulletPrefab;

        [Tooltip("Cooldown in seconds between each time the gun can be fired")]
        [SerializeField] private float rateOfFire = 1.0f;
        [SerializeField, Range(0.1f, 1.0f)] private float aimFireAccuracy = 1.0f;
        [SerializeField, Range(0.1f, 1.0f)] private float hipFireAccuracy = 0.0f;

        // Effects
        [Header("Effects")]
        [SerializeField] private GunEffects gunEffects;
        private ParticleSystem bulletHitParticles, muzzleFlashParticles;

        // EVENTS
        public event Action<Gun> OnReloaded;

        private void Awake() 
        {
            if(gunEffects)
            {
                bulletHitParticles = gunEffects.hitEffect ? Instantiate(gunEffects.hitEffect, Vector3.zero, Quaternion.identity, transform) : null;
                muzzleFlashParticles = gunEffects.muzzleFlashEffect ? Instantiate(gunEffects.muzzleFlashEffect, Vector3.zero, Quaternion.identity, bulletOrigin) : null;
                // ensure muzzle flash is locally zeroed
                muzzleFlashParticles.transform.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Shoots the gun.
        /// </summary>
        public override void Attack()
        {
            if (canUse && isEquipped)
            {
                Debug.Log("BANG!");
                muzzleFlashParticles.Play();
                
                RaycastHit hit;
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f);

                if(hit.collider)
                {
                    bulletHitParticles.transform.position = hit.point;
                    bulletHitParticles.transform.forward = hit.normal;
                    bulletHitParticles.Play();
                }

                // Instantiate Bullet in direction accounting for aim type accuracy

                CurrentAmmo -= 1;
                StartCoroutine(AttackCooldown());
            }
        }

        protected IEnumerator AttackCooldown()
        {
            canUse = false;

            yield return new WaitForSeconds(rateOfFire);

            canUse = true;
        }

        /// <summary>
        /// Reloads the weapon over time.
        /// </summary>
        public void Reload(int ammo)
        {
            if (!IsReloading)
            {
                StartCoroutine(ReloadCoroutine(ammo));
            }
        }

        protected IEnumerator ReloadCoroutine(int ammo)
        {
            canUse = false;
            IsReloading = true;

            yield return new WaitForSeconds(reloadTime);

            CurrentAmmo = Mathf.Min(CurrentAmmo + ammo, MaxAmmoCapacity);

            IsReloading = false;
            canUse = true;
            OnReloaded.Invoke(this);
        }
    }
}
