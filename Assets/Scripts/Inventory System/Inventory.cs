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
        public event Action<ItemData> OnCantAddItem;

        private void Awake()
        {
            // Singleton
            Instance = this;

            inventory = new List<Item>();

            player = FindObjectOfType<PlayerController>();
        }

        public bool AddItem(ItemData itemData, int amount)
        {
            bool added = false;
            Item itemToAdd = null;

            // Add to existing Item if stackable
            if (itemData.isStackable)
            {
                itemToAdd = FindItem(itemData);

                if (itemToAdd != null)
                {
                    added = itemToAdd.AddItemAmount(amount) > 0;
                }
            }
            
            if(!added && inventory.Count < MaxInventorySize)
            {
                itemToAdd = new Item(itemData, amount);
                inventory.Add(itemToAdd);
                added = true;
            }

            if (!added)
            {
                OnCantAddItem?.Invoke(itemData);
            }
            else
            {
                OnItemAdded?.Invoke(itemToAdd, amount);
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
