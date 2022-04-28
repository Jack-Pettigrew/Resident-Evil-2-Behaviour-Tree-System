using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;
using DD.Core.Items;

namespace DD.Core.Combat
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: Header("Weapon")]
        public bool isEquipped = false;
        public bool canUse = false;
        [SerializeField] protected int weaponDamage = 1;

        /// <summary>
        /// The attacking logic for the weapon.
        /// </summary>
        public abstract void Attack();
        
        /// <summary>
        /// The logic for the weapon action.
        /// </summary>
        public abstract void UseWeaponAction();

        public void SetCanUse(bool toggle) => canUse = toggle;
    }
}
