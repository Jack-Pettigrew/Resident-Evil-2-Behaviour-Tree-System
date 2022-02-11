using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;

namespace DD.Core.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        #region Singleton
            public static Inventory Instance {get; private set;}

            private void Awake() {
                Instance = this;
            }
        #endregion
        
        // INVENTORY
        private List<ItemSlot> inventory = new List<ItemSlot>();

        [field: SerializeField] public int MaxInventorySize { private set; get; }

        // EVENTS
        public Action<ItemData> OnItemAdded;
        public Action<ItemData> OnCantAddItem;

        public bool AddItem(ItemData itemData)
        {
            ItemSlot slot = FindItem(itemData);
            bool added = false;

            // Check for existing slot
            if(slot != null)
            {
                added = slot.AddItem(1);
            }
            else if(inventory.Count < MaxInventorySize)
            {
                slot = new ItemSlot(itemData);
                slot.OnItemDepleted += RemoveItem;

                inventory.Add(slot);
                added = true;
            }

            if(!added)
            {
                OnCantAddItem?.Invoke(itemData);
            }
            else
            {
                OnItemAdded.Invoke(itemData);
            }

            return added;
        }

        public void ReduceItem(ItemData itemData, int amount)
        {
            ItemSlot slot = FindItem(itemData);

            if(slot != null)
            {
                slot.ReduceItem(amount);
            }
        }

        public void RemoveItem(ItemSlot itemSlot)
        {
            if(itemSlot == null) return;
            
            if(inventory.Contains(itemSlot))
            {
                inventory.Remove(itemSlot);
            }
        }

        public void RemoveItem(ItemData itemData)
        {
            RemoveItem(FindItem(itemData));
        }

        public bool HasItem(ItemData itemData)
        {
            return FindItem(itemData) != null ? true : false;
        }

        /// <summary>
        /// Finds the Item within the inventory.
        /// </summary>
        /// <param name="itemData">The item data of the item to find.</param>
        /// <returns>The item slot the item is in OR null if not found.</returns>
        private ItemSlot FindItem(ItemData itemData)
        {
            foreach (ItemSlot slot in inventory)
            {
                if (slot.ItemData == itemData)
                {
                    return slot;
                }
            }

            return null;
        }
    }
}
