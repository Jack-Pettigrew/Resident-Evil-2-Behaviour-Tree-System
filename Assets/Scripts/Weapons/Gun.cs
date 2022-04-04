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
        private bool canShoot = true;
        [Tooltip("Cooldown in seconds between each time the gun can be fired")]
        [SerializeField] private float rateOfFire = 1.0f;
        [SerializeField, Range(0.1f, 1.0f)] private float aimFireAccuracy = 1.0f;
        [SerializeField, Range(0.1f, 1.0f)] private float hipFireAccuracy = 0.0f;

        // EVENTS
        public event Action<Gun> OnReloaded;

        /// <summary>
        /// Shoots the gun.
        /// </summary>
        public override void Attack()
        {
            if (canShoot)
            {
                Debug.Log("BANG!");
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.red, 10.0f);
                
                // Instantiate Bullet in direction accounting for aim type accuracy
                
                CurrentAmmo -= 1;
                StartCoroutine(AttackCooldown());
            }
        }

        protected IEnumerator AttackCooldown()
        {
            canShoot = false;

            yield return new WaitForSeconds(rateOfFire);

            canShoot = true;
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
            canShoot = false;
            IsReloading = true;

            yield return new WaitForSeconds(reloadTime);

            CurrentAmmo = Mathf.Min(CurrentAmmo + ammo, MaxAmmoCapacity);

            IsReloading = false;
            canShoot = true;
            OnReloaded.Invoke(this);
        }
    }
}
