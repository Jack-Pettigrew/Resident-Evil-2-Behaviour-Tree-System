using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.UI;

namespace DD.Core.Items
{
    public class HealingItem : Item
    {
        public HealingItem(ItemData itemData, int amountOfItem) : base(itemData, amountOfItem)
        { }

        public override List<ContextMenuOption> GetContextOptions()
        {
            List<ContextMenuOption> options = new List<ContextMenuOption>();
            options.Add(new ContextMenuOption("Use", () => Use()));

            return options;
        }

        public override void Use()
        {
            ItemUser.Instance.UseItem(ItemData);
        }

        public override bool Combine(Item combiningItem)
        {
            if(combiningItem is HealingItem) return false;

            AddItemAmount(combiningItem.ItemAmount);
            return true;
        }

    }
}
