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
        [SerializeField] private List<ItemSlot> inventory = new List<ItemSlot>();

        [field: SerializeField] public int MaxInventorySize { private set; get; }

        // EVENTS
        public event Action<ItemSlot> OnNewInventoryItem;     // When a new item has been given an item slot
        public event Action<ItemData> OnItemAdded;            // When an item has been added to the inventory
        public event Action<ItemData> OnCantAddItem;          // When an item failed being added to the inventory

        private void Awake()
        {
            // Singleton
            Instance = this;

            player = FindObjectOfType<PlayerController>();

            for (int i = 0; i < MaxInventorySize; i++)
            {
                inventory.Add(new ItemSlot());
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && inventory.Count > 0)
            {
                DropItem(inventory[0].ItemData);
            }
        }

        public bool AddItem(ItemData itemData)
        {
            ItemSlot slot = FindItem(itemData);
            bool added = false;

            // Check for existing slot
            if (slot != null && itemData.isStackable)
            {
                added = slot.AddItem(1);
            }
            else
            {
                // Add to empty slot if able
                slot = inventory.Find(x => x.Amount == 0);

                if (slot != null)
                {
                    slot.SetItem(itemData);
                    OnNewInventoryItem?.Invoke(slot);
                    added = true;
                }
            }

            if (!added)
            {
                OnCantAddItem?.Invoke(itemData);
            }
            else
            {
                OnItemAdded?.Invoke(itemData);
            }

            return added;
        }

        public void ReduceItem(ItemData itemData, int amount)
        {
            ItemSlot slot = FindItem(itemData);

            if (slot != null)
            {
                slot.ReduceItem(amount);
            }
        }

        public void RemoveItem(ItemData itemData, int amount = 1)
        {
            FindItem(itemData).ReduceItem(amount);
        }

        public void DropItem(ItemData itemData, int amount = 1)
        {
            ItemSlot slot = FindItem(itemData);
            if (slot != null)
            {
                slot.ReduceItem(amount);
                Instantiate(itemData.itemPrefab, player.transform.position + (player.transform.forward + player.transform.right * UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
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
