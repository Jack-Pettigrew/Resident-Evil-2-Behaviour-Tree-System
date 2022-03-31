using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Combat
{
    public class Gun : Weapon
    {
        [field: Header("Ammo")]
        [field: SerializeField] public int CurrentAmmo { private set; get; }
        [field: SerializeField] public int MaxAmmoCapacity { private set; get; }
        public bool IsReloading { private set; get; }
        [SerializeField] private float reloadTime = 1.0f;

        [Header("Firing")]
        private bool canShoot = true;
        [SerializeField] private float rateOfFire = 1.0f;

        // EVENTS
        public event Action<Gun> OnReloaded;

        /// <summary>
        /// Shoots the gun.
        /// </summary>
        public override void Attack()
        {
            if(canShoot)
            {
                Debug.Log("BANG!");
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
            if(!IsReloading)
            {
                StartCoroutine(ReloadCoroutine(ammo));
            }
        }

        protected IEnumerator ReloadCoroutine(int ammo)
        {
            IsReloading = true;

            yield return new WaitForSeconds(reloadTime);

            CurrentAmmo = Mathf.Min(CurrentAmmo + ammo, MaxAmmoCapacity);

            OnReloaded.Invoke(this);
            IsReloading = false;
        }
    }
}
