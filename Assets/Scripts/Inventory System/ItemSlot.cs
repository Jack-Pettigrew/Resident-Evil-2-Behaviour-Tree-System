using System;
using System.Collections;
using System.Collections.Generic;
using DD.Core.Items;

namespace DD.Core.InventorySystem
{
    public class ItemSlot
    {
        public ItemData ItemData { private set; get; }
        public int Amount { private set; get; }

        public Action<ItemSlot> OnItemDepleted;

        public ItemSlot(ItemData itemData, int amount = 1)
        {
            this.ItemData = itemData;
            Amount = amount;
        }

        public bool AddItem(int amount)
        {
            if(Amount + amount > ItemData.maxStackSize)
            {
                return false;
            }

            Amount += amount;
            return true;
        }

        /// <summary>
        /// Reduces the item by tghe amount given.
        /// </summary>
        /// <param name="amount">Amount to reduce the item by.</param>
        public void ReduceItem(int amount)
        {
            Amount -= amount;

            if(Amount <= 0)
            {
                OnItemDepleted?.Invoke(this);
            }
        }

        public bool UseItem()
        {
            return false;
        }
    }
}
