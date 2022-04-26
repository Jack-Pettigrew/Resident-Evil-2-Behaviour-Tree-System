using System.Collections;
using System.Collections.Generic;
using DD.UI;
using UnityEngine;

namespace DD.Core.Items
{
    public class AmmoItem : Item
    {
        public AmmoItem(ItemData itemData, int amountOfItem) : base(itemData, amountOfItem)
        {
        }

        public override List<ContextMenuOption> GetContextOptions()
        {
            List<ContextMenuOption> options = new List<ContextMenuOption>();
            options.Add(new ContextMenuOption("Combine", () => Use()));

            return options;
        }

        public override void Use()
        {

        }
    }
}
