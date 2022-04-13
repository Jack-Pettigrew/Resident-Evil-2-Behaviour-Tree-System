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
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField, Min(0.0f)] private float bulletSpeed = 10.0f;

        // Bullet Pool
        private List<Bullet> bulletPool;
        private int bulletPoolIndex = 0;

        [Tooltip("Cooldown in seconds between each time the gun can be fired")]
        [SerializeField] private float rateOfFire = 1.0f;

        // Effects
        [Header("Effects")]
        [SerializeField] private GunEffects gunEffects;
        private ParticleSystem bulletHitParticles, muzzleFlashParticles;

        // Coroutines
        private Coroutine attackCooldownCoroutine;
        private Coroutine reloadCoroutine;

        // EVENTS
        public event Action<Gun> OnReloaded;

        private void Awake() 
        {
            CurrentAmmo = MaxAmmoCapacity;

            if(bulletPrefab)
            {
                bulletPool = new List<Bullet>();

                for (int i = 0; i < MaxAmmoCapacity; i++)
                {
                    bulletPool.Add(Instantiate<Bullet>(bulletPrefab, Vector3.zero, Quaternion.identity));
                    bulletPool[i].gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("No Bullet Prefab selected.");
            }
            
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
            if (canUse && isEquipped && CurrentAmmo > 0)
            {                
                // Fire Ray
                RaycastHit hit;
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f);

                // Weapon Effects
                muzzleFlashParticles.Play();
                if(hit.collider)
                {
                    bulletHitParticles.transform.position = hit.point;
                    bulletHitParticles.transform.forward = hit.normal;
                    bulletHitParticles.Play();
                }

                // Fire bullet from pool
                bulletPool[bulletPoolIndex].Fire(weaponDamage, bulletOrigin.position, bulletOrigin.forward, bulletSpeed);
                bulletPoolIndex = (bulletPoolIndex + 1) % bulletPool.Count;

                CurrentAmmo -= 1;
                attackCooldownCoroutine = StartCoroutine(AttackCooldown());
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
            if (canUse && !IsReloading && CurrentAmmo < MaxAmmoCapacity)
            {
                reloadCoroutine = StartCoroutine(ReloadCoroutine(ammo));
            }
        }

        protected IEnumerator ReloadCoroutine(int ammo)
        {
            if(attackCooldownCoroutine != null) StopCoroutine(attackCooldownCoroutine);

            canUse = false;
            IsReloading = true;

            yield return new WaitForSeconds(reloadTime);

            CurrentAmmo = Mathf.Min(CurrentAmmo + ammo, MaxAmmoCapacity);

            IsReloading = false;
            canUse = true;
            OnReloaded?.Invoke(this);
        }
    }
}
