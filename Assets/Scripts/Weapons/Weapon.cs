using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.InventorySystem;
using DD.Core.Items;

namespace DD.Core.Combat
{
    public abstract class Weapon : MonoBehaviour, IInteractable
    {
        [Header("Item")]
        [SerializeField] private ItemData itemData;

        [field: Header("Weapon")]
        [field: SerializeField] public WeaponType WeaponType { private set; get; }
        public bool IsOwned { private set; get; }
        public bool IsEquipped { private set; get; }
        [SerializeField] private int damage = 1;

        public void Interact()
        {
            Inventory.Instance.AddItem(itemData);
        }
    }

    public enum WeaponType
    {
        Primary,
        Secondary,
        Ordinance
    }
}