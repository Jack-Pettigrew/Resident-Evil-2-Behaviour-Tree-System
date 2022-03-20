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

        public event Action OnItemUpdated;
        public event Action OnItemDepleted;

        public ItemSlot()
        {
            ItemData = null;
            Amount = 0;
        }

        /// <summary>
        /// Sets the item this ItemSlot will hold.
        /// </summary>
        /// <param name="itemData">The ItemData of the new item.</param>
        /// <param name="amount">The amount of this item to hold.</param>
        /// <returns>Successful?</returns>
        public bool SetItem(ItemData itemData, int amount = 1)
        {
            if(itemData == null) return false;

            this.ItemData = itemData;
            Amount = amount;
            return true;
        }

        /// <summary>
        /// Adds to the amount this ItemSlot holds.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Successful?</returns>
        public bool AddItem(int amount)
        {
            if(Amount + amount > ItemData.maxStackSize)
            {
                return false;
            }

            Amount += amount;
            OnItemUpdated?.Invoke();
            return true;
        }

        /// <summary>
        /// Reduces the item by tghe amount given.
        /// </summary>
        /// <param name="amount">Amount to reduce the item by.</param>
        public void ReduceItem(int amount)
        {
            Amount -= amount;
            OnItemUpdated?.Invoke();

            if(Amount <= 0)
            {
                ItemData = null;
                OnItemDepleted?.Invoke();
            }
        }
    }
}
