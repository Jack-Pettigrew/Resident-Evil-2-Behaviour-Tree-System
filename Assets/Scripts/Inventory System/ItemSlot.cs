using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;

namespace DD.Systems.InventorySystem
{
    public class ItemSlot
    {
        public ItemData Item { private set; get; }
        public int ItemQuantity { private set; get; }

        // Events
        public event Action<ItemSlot> OnItemSlotEmptied;

        public ItemSlot(ItemData item, int amount)
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
            ItemQuantity -= amountToRemove;

            if(ItemQuantity <= 0)
            {
                OnItemSlotEmptied?.Invoke(this);
            }
        }
    }
}
