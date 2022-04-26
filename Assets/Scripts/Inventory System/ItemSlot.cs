using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;

namespace DD.Systems.InventorySystem
{
    public class ItemSlot
    {
        public Item Item { private set; get; }
        public int ItemQuantity { private set; get; }

        public ItemSlot(Item item, int amount)
        {
            this.Item = item;
            this.ItemQuantity = amount;
        }

        public void AddItem(int amountToAdd)
        {
            if(amountToAdd < 0) return;
            
            ItemQuantity += amountToAdd;
        }

        public void RemoveItem(int amountToRemove)
        {
            if(amountToRemove < 0) return;

            ItemQuantity -= amountToRemove;
        }
    }
}
