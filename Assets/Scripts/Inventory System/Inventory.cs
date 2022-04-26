using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core.Items;

namespace DD.Systems.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }

        // PLAYER
        private PlayerController player;

        // INVENTORY
        public List<ItemSlot> inventory { private set; get; }
        [field: SerializeField] public int MaxInventorySize { private set; get; }

        // EVENTS
        /// <summary>
        /// When the inventory is updated in general
        /// </summary>
        public event Action OnInvetoryUpdated;

        /// <summary> 
        /// When a new ItemSlot has been given an ItemSlot slot and the amount added
        /// </summary>
        public event Action<Item, int> OnItemAdded;

        /// <summary>
        /// When an ItemSlot failed being added to the inventory
        /// </summary>
        public event Action<Item> OnCantAddItem;

        private void Awake()
        {
            // Singleton
            Instance = this;

            inventory = new List<ItemSlot>();

            player = FindObjectOfType<PlayerController>();
        }

        // GET ITEM FUNCTION SIMILAR TO ITEM DATABASE GENERIC WHERE T : ITEM ????????????

        public bool AddItem(Item item, int amountToAdd)
        {
            bool added = false;

            // Add to existing ItemSlot if stackable
            if (item.isStackable)
            {
                ItemSlot existingItemSlot = FindItemSlot(item);

                if (existingItemSlot != null)
                {
                    existingItemSlot.AddItem(amountToAdd);
                    added = true;
                }
            }
            
            // Add new ItemSlot
            if(!added && inventory.Count < MaxInventorySize)
            {
                inventory.Add(new ItemSlot(item, amountToAdd));
                added = true;
            }

            if (!added)
            {
                OnCantAddItem?.Invoke(item);
            }
            else
            {
                OnItemAdded?.Invoke(item, amountToAdd);
            }

            return added;
        }

        public void RemoveItem(Item item, int amount = 1)
        {
            ItemSlot ItemSlot = FindItemSlot(item);
            ItemSlot.RemoveItem(amount);
            
            if(ItemSlot.ItemQuantity <= 0)
            {
                inventory.Remove(ItemSlot);
            }
            
            OnInvetoryUpdated?.Invoke();
        }

        public void DropItem(Item item, int amount = 1)
        {
            if(!item.isDroppable) return;

            ItemSlot itemSlot = FindItemSlot(item);
            if (itemSlot != null)
            {
                WorldItem worldItem = Instantiate<WorldItem>(itemSlot.Item.itemPrefab, player.transform.position + (player.transform.forward + player.transform.right * UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
                worldItem.SetItemQuantity(amount);
                RemoveItem(item, amount);
            }
        }

        public bool HasItem(Item item)
        {
            return FindItemSlot(item) != null ? true : false;
        }

        /// <summary>
        /// Finds the ItemSlot within the inventory.
        /// </summary>
        /// <param name="ItemSlotData">The ItemSlot data of the ItemSlot to find.</param>
        /// <returns>The ItemSlot slot the ItemSlot is in OR null if not found.</returns>
        public ItemSlot FindItemSlot(Item item)
        {
            foreach (ItemSlot ItemSlot in inventory)
            {
                if (ItemSlot.Item == item)
                {
                    return ItemSlot;
                }
            }

            return null;
        }
    }
}
