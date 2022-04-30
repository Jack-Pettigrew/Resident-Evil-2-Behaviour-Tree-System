using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;
using DD.Core.Items;

namespace DD.Core.Combat
{
    [RequireComponent(typeof(WorldItem))]
    public abstract class Weapon : MonoBehaviour
    {
        public WorldItem WorldItem { private set; get; }

        [field: Header("Weapon")]
        public bool isEquipped { protected set; get; }
        public bool CanUse { protected set; get; }
        [SerializeField] protected int weaponDamage = 1;

        protected virtual void Awake() {
            WorldItem = GetComponent<WorldItem>();
        }
        
        /// <summary>
        /// The attacking logic for the weapon.
        /// </summary>
        public abstract void Attack();
        
        /// <summary>
        /// The logic for the weapon action.
        /// </summary>
        public abstract void UseWeaponAction();

        public void SetCanUse(bool toggle) => CanUse = toggle;
        public void SetEquipped(bool equipped)
        {
            isEquipped = equipped;
            WorldItem.CanInteract = !equipped;
        }
    }
}
