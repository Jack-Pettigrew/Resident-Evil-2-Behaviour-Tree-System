using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Systems.InventorySystem;
using DD.Animation;
using DD.Core.Control;
using UnityEngine.Events;

namespace DD.Core.Combat
{
    [RequireComponent(typeof(WorldItem))]
    public class Gun : Weapon
    {        
        [field: Header("Ammo")]
        [field: SerializeField] public AmmoItem GunAmmoItem { private set; get; }
        [field: SerializeField] public int MaxAmmoCapacity { private set; get; }
        [field: SerializeField] public bool HasInfiniteAmmo { get; private set; }
        public int CurrentAmmo { private set; get; }
        public bool IsReloading { private set; get; }
        [SerializeField] private float reloadTime = 1.0f;

        // Firing
        [Header("Firing")]
        [SerializeField] private Transform bulletOrigin;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField, Min(0.0f)] private float bulletSpeed = 10.0f;
        [SerializeField] private LayerMask shootingLayerMask;

        // Bullet Pool
        private Bullet[] bulletPool;
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
        [Space]
        public UnityEvent OnShoot;
        public event Action OnReloading;
        public event Action OnReloaded;

        protected void Awake()
        {
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
                bulletPool = new Bullet[MaxAmmoCapacity];

                for (int i = 0; i < MaxAmmoCapacity; i++)
                {
                    bulletPool[i] = Instantiate<Bullet>(bulletPrefab, Vector3.zero, Quaternion.identity);
                    bulletPool[i].gameObject.SetActive(false);
                    bulletPool[i].SetBulletHitParticleSystem(bulletHitParticles);
                }
            }
            else
            {
                Debug.LogError("No Bullet Prefab selected.");
            }

            // Animation Subbing
            FindObjectOfType<PlayerAnimationController>()?.InitAnimationEvent(this);
        }

        private void OnDestroy() {
            // Remove associated bullets from scene
            foreach (Bullet bullet in bulletPool)
            {
                if(bullet)
                {
                    Destroy(bullet.gameObject);
                }
            }
        }

        /// <summary>
        /// Shoots the gun.
        /// </summary>
        public override void Attack()
        {
            if (InputManager.Instance.Aim && CanUse && CurrentAmmo > 0)
            {                
                // Fire Ray
                RaycastHit hit;
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f, shootingLayerMask);
                
                // Weapon Effects
                muzzleFlashParticles.Play();

                // Fire bullet from pool
                Vector3 bulletDirection = hit.collider ? hit.point - bulletOrigin.position : bulletOrigin.forward;

                bulletPool[bulletPoolIndex].transform.position = bulletOrigin.position;
                bulletPool[bulletPoolIndex].transform.rotation = Quaternion.LookRotation(bulletDirection);
                
                bulletPool[bulletPoolIndex].Fire(weaponDamage, bulletSpeed);
                bulletPoolIndex = (bulletPoolIndex + 1) % bulletPool.Length;

                CurrentAmmo -= HasInfiniteAmmo ? 0 : 1;
                attackCooldownCoroutine = StartCoroutine(AttackCooldown());
                OnShoot?.Invoke();
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
                ItemSlot itemSlot = Inventory.Instance.FindItemSlot(GunAmmoItem);

                if(itemSlot == null) return;

                reloadCoroutine = StartCoroutine(ReloadCoroutine(itemSlot));
            }
        }

        protected IEnumerator ReloadCoroutine(ItemSlot itemSlot)
        {
            if(attackCooldownCoroutine != null) 
            {
                StopCoroutine(attackCooldownCoroutine);
            }

            CanUse = false;
            IsReloading = true;
            OnReloading?.Invoke();

            yield return new WaitForSeconds(reloadTime);

            int ammoToAdd = Mathf.Min(MaxAmmoCapacity - CurrentAmmo, itemSlot.ItemQuantity);
            CurrentAmmo += ammoToAdd;
            itemSlot.RemoveItem(ammoToAdd);
            
            IsReloading = false;
            CanUse = true;
            OnReloaded?.Invoke();
        }

        public override void SubscribeAnimator(PlayerAnimationController animationController)
        {
            OnReloading += animationController.TriggerReload;

            OnShoot.AddListener(animationController.TriggerShoot);
        }

        public override void UnsubscribeAnimator(PlayerAnimationController animationController)
        {
            OnReloading -= animationController.TriggerReload;

            OnShoot.RemoveListener(animationController.TriggerShoot);
        }
    }
}
