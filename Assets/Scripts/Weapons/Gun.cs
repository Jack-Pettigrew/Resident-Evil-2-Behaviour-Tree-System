using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Systems.InventorySystem;

namespace DD.Core.Combat
{
    public class Gun : Weapon
    {
        [Header("Ammo")]
        [SerializeField] private AmmoItem gunAmmoItem;
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

        protected override void Awake() 
        {
            base.Awake();

            CurrentAmmo = MaxAmmoCapacity;

            // Particles
            if(gunEffects)
            {
                bulletHitParticles = gunEffects.hitEffect ? Instantiate(gunEffects.hitEffect, Vector3.zero, Quaternion.identity, transform) : null;
                muzzleFlashParticles = gunEffects.muzzleFlashEffect ? Instantiate(gunEffects.muzzleFlashEffect, Vector3.zero, Quaternion.identity, bulletOrigin) : null;
                // ensure muzzle flash is locally zeroed
                muzzleFlashParticles.transform.localPosition = Vector3.zero;
            }

            // Bullet Instantiation
            if(bulletPrefab)
            {
                bulletPool = new List<Bullet>();

                for (int i = 0; i < MaxAmmoCapacity; i++)
                {
                    bulletPool.Add(Instantiate<Bullet>(bulletPrefab, Vector3.zero, Quaternion.identity));
                    bulletPool[i].gameObject.SetActive(false);
                    bulletPool[i].SetBulletHitParticleSystem(bulletHitParticles);
                }
            }
            else
            {
                Debug.LogError("No Bullet Prefab selected.");
            }
        }

        /// <summary>
        /// Shoots the gun.
        /// </summary>
        public override void Attack()
        {
            if (CanUse && isEquipped && CurrentAmmo > 0)
            {                
                // Fire Ray
                RaycastHit hit;
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f);

                // Weapon Effects
                muzzleFlashParticles.Play();

                // Fire bullet from pool
                Vector3 bulletDirection = hit.collider ? hit.point - bulletOrigin.position : bulletOrigin.forward;
                bulletPool[bulletPoolIndex].Fire(weaponDamage, bulletOrigin.position, bulletDirection, bulletSpeed);
                bulletPoolIndex = (bulletPoolIndex + 1) % bulletPool.Count;

                CurrentAmmo -= 1;
                attackCooldownCoroutine = StartCoroutine(AttackCooldown());
            }
        }

        protected IEnumerator AttackCooldown()
        {
            CanUse = false;

            yield return new WaitForSeconds(rateOfFire);

            CanUse = true;
        }

        /// <summary>
        /// Reloads the weapon.
        /// </summary>
        public override void UseWeaponAction()
        {
            if (CanUse && !IsReloading && CurrentAmmo < MaxAmmoCapacity)
            {
                // Gets the amount of ammo for this gun from the inventory
                ItemSlot itemSlot = Inventory.Instance.FindItemSlot(gunAmmoItem);

                if(itemSlot == null) return;

                reloadCoroutine = StartCoroutine(ReloadCoroutine(Mathf.Min(CurrentAmmo + itemSlot.ItemQuantity, MaxAmmoCapacity)));
            }
        }

        protected IEnumerator ReloadCoroutine(int ammo)
        {
            if(attackCooldownCoroutine != null) StopCoroutine(attackCooldownCoroutine);

            CanUse = false;
            IsReloading = true;

            yield return new WaitForSeconds(reloadTime);

            CurrentAmmo = ammo;
            IsReloading = false;
            CanUse = true;
            OnReloaded?.Invoke(this);
        }
    }
}
