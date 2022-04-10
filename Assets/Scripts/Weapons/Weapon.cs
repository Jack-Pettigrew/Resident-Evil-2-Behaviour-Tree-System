using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;
using DD.Core.Items;

namespace DD.Core.Combat
{
    public abstract class Weapon : MonoBehaviour, IInteractable
    {
        [Header("Item")]
        [SerializeField] private ItemData itemData;

        [field: Header("Weapon")]
        [field: SerializeField] public WeaponType WeaponType { private set; get; }
        public bool isOwned = false;
        public bool isEquipped = false;
        public bool canUse = false;
        [SerializeField] private int damage = 1;

        /// <summary>
        /// The attacking logic for the weapon.
        /// </summary>
        public abstract void Attack();

        public void SetCanUse(bool toggle) => canUse = toggle;

        public void Interact()
        {
            if(isOwned) return;

            Inventory.Instance.AddItem(itemData);
        }
    }

    public enum WeaponType
    {
        Gun,
        Melee,
        Throwable
    }
}
