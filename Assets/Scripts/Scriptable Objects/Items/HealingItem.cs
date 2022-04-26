using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "Healing Item", menuName = "Items/Healing Item", order = 1)]
    public class HealingItem : Item
    {
        [field: Header("Healing Properties")]
        [field: SerializeField] public int HealAmount { private set; get; }

        public override void Use()
        {
            PlayerHealer.Instance.ConsumeHealItem(this);
            Inventory.Instance.RemoveItem(this);
        }
    }
}