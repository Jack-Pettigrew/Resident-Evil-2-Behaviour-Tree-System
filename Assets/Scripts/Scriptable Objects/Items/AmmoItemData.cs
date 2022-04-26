using System.Collections;
using System.Collections.Generic;
using DD.Systems.InventorySystem;
using DD.Core.Combat;
using UnityEngine;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "AmmoItem", menuName = "Items/Ammo Item", order = 2)]
    public class AmmoItemData : ItemData
    {
        [Header("Ammo Properties")]
        public AmmoType ammoType;
        public WeaponType weaponType;

        public override Item CreateItemInstance(int amountOfItem)
        {
            return new AmmoItem(this, amountOfItem);
        }
    }
}