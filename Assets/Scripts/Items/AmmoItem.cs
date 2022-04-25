using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{
    public class AmmoItem : Item
    {
        public AmmoItem(ItemData itemData, int amountOfItem) : base(itemData, amountOfItem)
        {
        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}
