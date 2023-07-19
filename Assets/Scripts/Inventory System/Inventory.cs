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
        private static Inventory instance;
        public static Inventory Instance {
            get {
                if(instance == null)
                {
                    instance = FindObjectOfType<Inventory>();
                }

                return instance;
            } 
        }

        // PLAYER
        private PlayerController player;

        // INVENTORY
        public List<ItemSlot> inventory { private set; get; }
        [field: SerializeField] public int MaxInventorySize { private set; get; }

        // EVENTS
        /// <summary>
        /// When the inventory is updated in general
        /// </summary>
        public event Action OnInventoryUpdated;

        /// <summary> 
        /// When a new ItemSlot has been given an ItemSlot slot and the amount added
        /// </summary>
        public event Action<ItemSlot> OnItemAdded;

        /// <summary>
        /// When an Item has been dropped from the inventory
        /// </summary>
        public event Action<ItemData> OnItemDropped;

        /// <summary>
        /// When an ItemSlot failed being added to the inventory
        /// </summary>
        public event Action<ItemData> OnCantAddItem;

        private void Awake()
        {
            inventory = new List<ItemSlot>();

            player = FindObjectOfType<PlayerController>();
        }

        // GET ITEM FUNCTION SIMILAR TO ITEM DATABASE GENERIC WHERE T : ITEM ????????????

        public bool AddItem(ItemData item, int amountToAdd)
        {
            bool added = false;

            ItemSlot itemSlot = null;

            // Add to existing ItemSlot if stackable
            if (item.isStackable)
            {
                itemSlot = FindItemSlot(item);

                if (itemSlot != null)
                {
                    itemSlot.AddItem(amountToAdd);
                    added = true;
                }
            }
            
            // Add new ItemSlot
            if(!added && inventory.Count < MaxInventorySize)
            {
                itemSlot = new ItemSlot(item, amountToAdd);
                itemSlot.OnItemSlotEmptied += RemoveItemSlot;
                
                inventory.Add(itemSlot);
                added = true;
            }

            if (!added)
            {
                OnCantAddItem?.Invoke(item);
            }
            else
            {
                OnItemAdded?.Invoke(itemSlot);
                GlobalEvents.OnPickupItem?.Invoke(item);
            }

            return added;
        }

        public void RemoveItem(ItemData item, int amount = 1)
        {
            ItemSlot ItemSlot = FindItemSlot(item);
            ItemSlot.RemoveItem(amount);
            
            if(ItemSlot.ItemQuantity <= 0)
            {
                inventory.Remove(ItemSlot);
            }
            
            OnInventoryUpdated?.Invoke();
        }

        public void DropItem(ItemData item, int amount = 1)
        {
            if(!item.isDroppable) return;

            ItemSlot itemSlot = FindItemSlot(item);

            if (itemSlot != null)
            {
                WorldItem worldItem = Instantiate<WorldItem>(itemSlot.ItemData.itemPrefab, player.transform.position + (player.transform.forward + player.transform.right * UnityEngine.Random.Range(-1.0f, 1.0f)), Quaternion.identity);
                worldItem.SetItemQuantity(amount);
                RemoveItem(item, amount);
                OnItemDropped?.Invoke(itemSlot.ItemData);
            }
        }

        public void RemoveItemSlot(ItemSlot itemSlot)
        {
            inventory.Remove(itemSlot);
        }

        public bool HasItem(ItemData item)
        {
            return FindItemSlot(item) != null ? true : false;
        }

        /// <summary>
        /// Finds the ItemSlot within the inventory.
        /// </summary>
        /// <param name="ItemSlotData">The ItemSlot data of the ItemSlot to find.</param>
        /// <returns>The ItemSlot slot the ItemSlot is in OR null if not found.</returns>
        public ItemSlot FindItemSlot(ItemData item)
        {
            foreach (ItemSlot ItemSlot in inventory)
            {
                if (ItemSlot.ItemData == item)
                {
                    return ItemSlot;
                }
            }

            return null;
        }
    }
}
