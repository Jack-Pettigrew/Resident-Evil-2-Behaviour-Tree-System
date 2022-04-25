using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Core.Control;

namespace DD.Systems.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }

        // PLAYER
        private PlayerController player;

        // INVENTORY
        public List<Item> inventory { private set; get; }
        [field: SerializeField] public int MaxInventorySize { private set; get; }

        // EVENTS
        /// <summary>
        /// When the inventory is updated in general
        /// </summary>
        public event Action OnInvetoryUpdated;

        /// <summary> 
        /// When a new item has been given an item slot and the amount added
        /// </summary>
        public event Action<Item, int> OnItemAdded;

        /// <summary>
        /// When an item failed being added to the inventory
        /// </summary>
        public event Action<Item> OnCantAddItem;

        private void Awake()
        {
            // Singleton
            Instance = this;

            inventory = new List<Item>();

            player = FindObjectOfType<PlayerController>();
        }

        public bool AddItem(Item item)
        {
            bool added = false;

            // Add to existing Item if stackable
            if (item.ItemData.isStackable)
            {
                Item existingItem = FindItem(item.ItemData);

                if (existingItem != null)
                {
                    added = existingItem.AddItemAmount(item.ItemAmount) > 0;
                }
            }
            
            // Add new item
            if(!added && inventory.Count < MaxInventorySize)
            {
                inventory.Add(item);
                added = true;
            }

            if (!added)
            {
                OnCantAddItem?.Invoke(item);
            }
            else
            {
                OnItemAdded?.Invoke(item, item.ItemAmount);
            }

            return added;
        }

        public void RemoveItem(ItemData itemData, int amount = 1)
        {
            Item item = FindItem(itemData);
            item.RemoveItemAmount(amount);
            
            if(item.ItemAmount <= 0)
            {
                inventory.Remove(item);
            }
            
            OnInvetoryUpdated?.Invoke();
        }

        public void DropItem(ItemData itemData, int amount = 1)
        {
            if(!itemData.isDroppable) return;

            Item item = FindItem(itemData);
            if (item != null)
            {
                WorldItem worldItem = Instantiate<WorldItem>(itemData.itemPrefab, player.transform.position + (player.transform.forward + player.transform.right * UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
                worldItem.SetItemAmount(amount);
                RemoveItem(itemData, amount);
            }
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
        public Item FindItem(ItemData itemData)
        {
            foreach (Item item in inventory)
            {
                if (item.ItemData == itemData)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
