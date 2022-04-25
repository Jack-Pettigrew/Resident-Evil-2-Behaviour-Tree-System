using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{
    public class HealingItem : Item
    {
        public HealingItem(ItemData itemData, int amountOfItem) : base(itemData, amountOfItem)
        { }

        public override void Use()
        {
        }
    }
}
